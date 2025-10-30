using Business.Interfaces.IJWT;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.Auth.RestPasword;
using Entity.DTOs.Default.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Web.Infrastructure;
using Business.Interfaces.IBusinessImplements.Security;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IToken _tokenService;
        private readonly IAuthCookieFactory _cookieFactory;
        private readonly JwtSettings _jwt;
        private readonly CookieSettings _cookieSettings;

        public AuthController(
            IAuthService authService,
            IToken tokenService,
            IAuthCookieFactory cookieFactory,
            IOptions<JwtSettings> jwtOptions,
            IOptions<CookieSettings> cookieOptions,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _tokenService = tokenService;
            _cookieFactory = cookieFactory;
            _jwt = jwtOptions.Value;
            _cookieSettings = cookieOptions.Value;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registrarse([FromBody] RegisterDto dto)
        {
            await _authService.RegisterAsync(dto);
            return Ok(new { isSuccess = true });
        }

        /// <summary>Login: genera access + refresh + csrf, guarda cookies HttpOnly.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
        {
            var (access, refresh, csrf) = await _tokenService.GenerateTokensAsync(dto);

            var now = DateTime.UtcNow;

            Response.Cookies.Append(
                _cookieSettings.accessTokenName,
                access,
                _cookieFactory.AccessCookieOptions(now.AddMinutes(_jwt.AccessTokenExpirationMinutes)));

            Response.Cookies.Append(
                _cookieSettings.refreshTokenName,
                refresh,
                _cookieFactory.RefreshCookieOptions(now.AddDays(_jwt.RefreshTokenExpirationDays)));

            Response.Cookies.Append(
                _cookieSettings.csrfCookieName,
                csrf,
                _cookieFactory.CsrfCookieOptions(now.AddDays(_jwt.RefreshTokenExpirationDays)));

            return Ok(new { isSuccess = true, message = "Login exitoso" });
        }

        /// <summary>Renueva tokens (usa refresh cookie + comprobación CSRF double-submit).</summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            var refreshCookie = Request.Cookies[_cookieSettings.refreshTokenName];
            if (string.IsNullOrWhiteSpace(refreshCookie))
                return Unauthorized();

            if (!Request.Headers.TryGetValue("X-XSRF-TOKEN", out var headerValue))
                return Forbid();

            var csrfCookie = Request.Cookies[_cookieSettings.csrfCookieName];
            if (string.IsNullOrWhiteSpace(csrfCookie) || csrfCookie != headerValue)
                return Forbid();

            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var (newAccess, newRefresh) = await _tokenService.RefreshAsync(refreshCookie, remoteIp);

            var now = DateTime.UtcNow;

            // Borrar cookies anteriores con mismas opciones:
            Response.Cookies.Delete(_cookieSettings.accessTokenName, _cookieFactory.AccessCookieOptions(now));
            Response.Cookies.Delete(_cookieSettings.refreshTokenName, _cookieFactory.RefreshCookieOptions(now));

            // Escribir nuevas:
            Response.Cookies.Append(
                _cookieSettings.accessTokenName,
                newAccess,
                _cookieFactory.AccessCookieOptions(now.AddMinutes(_jwt.AccessTokenExpirationMinutes)));

            Response.Cookies.Append(
                _cookieSettings.refreshTokenName,
                newRefresh,
                _cookieFactory.RefreshCookieOptions(now.AddDays(_jwt.RefreshTokenExpirationDays)));

            return Ok(new { isSuccess = true });
        }

        /// <summary>Logout: revoca refresh token y borra cookies.</summary>
        [HttpPost("logout")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            var refreshCookie = Request.Cookies[_cookieSettings.refreshTokenName];
            if (!string.IsNullOrWhiteSpace(refreshCookie))
                await _tokenService.RevokeRefreshTokenAsync(refreshCookie);

            var now = DateTime.UtcNow;
            Response.Cookies.Delete(_cookieSettings.accessTokenName, _cookieFactory.AccessCookieOptions(now));
            Response.Cookies.Delete(_cookieSettings.refreshTokenName, _cookieFactory.RefreshCookieOptions(now));
            Response.Cookies.Delete(_cookieSettings.csrfCookieName, _cookieFactory.CsrfCookieOptions(now));

            return Ok(new { message = "Sesión cerrada" });
        }

        /// <summary>/me: retorna el contexto del usuario actual.</summary>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                   ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(sub) || !int.TryParse(sub, out var userId))
                return Unauthorized("Token inválido o expirado.");

            var currentUserDto = await _authService.BuildUserContextAsync(userId);
            return Ok(currentUserDto);
        }

        [HttpPost("recuperar/enviar-codigo")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EnviarCodigoAsync([FromBody] RequestResetDto dto)
        {
            await _authService.RequestPasswordResetAsync(dto.email);
            return Ok(new { isSuccess = true, message = "Código enviado al correo (si el email es válido)" });
        }

        [HttpPost("recuperar/confirmar")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmarCodigo([FromBody] ConfirmResetDto dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok(new { isSuccess = true, message = "Contraseña actualizada" });
        }
    }
}

