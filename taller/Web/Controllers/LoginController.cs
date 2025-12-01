using Business.Interfaces.IBusinessImplements.Security;
using Business.Interfaces.IJWT;
using Entity.DTOs.Default.Email;
using Entity.DTOs.Default.LoginDto.response.LoginResultDto;
using Entity.DTOs.Default.LoginDto;
using Entity.DTOs.Default.RegisterRequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Custom;
using Microsoft.AspNetCore.Authentication;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class LoginController : ControllerBase
    {
        private readonly IToken _token;
        private readonly IUserService _userService;
        private readonly ILogger<LoginController> _logger;
        private readonly EncriptePassword _utilities;
        private readonly IAuthSessionService _svc;   // ADD: servicio de sesiones
        private readonly ISystemClock _clock;              // ADD: reloj (tu wrapper)

        //private readonly IServiceEmail _serviceEmail;
        //private readonly INotifyManager _notifyManager;

        public LoginController(
            EncriptePassword utilities,
            IToken token,
            ILogger<LoginController> logger,
            IUserService userService,
            IAuthSessionService svc,    // ADD
            ISystemClock clock                // ADD
        //, IServiceEmail serviceEmail,
        //, INotifyManager notifyManager
        )
        {
            _token = token;
            _userService = userService;
            _logger = logger;
            _utilities = utilities;
            _svc = svc;                // ADD
            _clock = clock;            // ADD
            //_serviceEmail = serviceEmail;
            //_notifyManager = notifyManager;
        }

        // ===========================
        // Registro de usuario normal
        // ===========================
        //[HttpPost("Registrarse")]
        //[ProducesResponseType(typeof(RegisterResponseDto), 200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> Registrarse([FromBody] RegisterRequestDto request)
        //{
        //    try
        //    {
        //        var result = await _userService.RegisterAsync(request);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex.Message;
        //        if (ex.InnerException != null) message += " | Inner: " + ex.InnerException.Message;
        //        return BadRequest(new { isSuccess = false, message });
        //    }
        //}

        [HttpPost("verify-code")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyEmailCodeRequestDto req)
        {
            var ok = await _userService.VerifyCodeAsync(req.Code);
            return ok
                ? Ok(new { isSuccess = true, message = "Correo verificado correctamente." })
                : BadRequest(new { isSuccess = false, message = "Código inválido o expirado." });
        }


        // ===========================
        // Login por Email + Password (JWT existente)
        // ===========================
        //[HttpPost("Email")]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(401)]
        //public async Task<IActionResult> LoginEmail([FromBody] EmailLoginDto login)
        //{
        //    try
        //    {
        //        var token = await _token.GenerateTokenEmail(login);
        //        return Ok(new { isSuccess = true, token });
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        return Unauthorized(new { isSuccess = false, message = "Credenciales inválidas." });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error en LoginEmail");
        //        return StatusCode(500, new { isSuccess = false, message = "Error interno." });
        //    }
        //}

        // ===========================
        // Login por Documento (SESIÓN con cookie, SIN JWT)
        // ===========================
        [HttpPost("documento")]
        [ProducesResponseType(typeof(LoginResultDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> logindocumento([FromBody] DocumentLoginDto login)
        {
            try
            {
                // -------------------------
                // 1. validar recapcha
                // -------------------------
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                // -------------------------
                // 2. lógica de usuario/sesión
                // -------------------------
                long? personId = null;
                // personId = await _userService.GetPersonIdByDocAsync(login.DocumentTypeId, login.DocumentNumber);

                var ua = Request.Headers.UserAgent.ToString();
                var sess = await _svc.CreateSessionAsync(personId, ip ?? "-", ua);

                // -------------------------
                // 3. cookie http-only
                // -------------------------
                Response.Cookies.Append("ph_session", sess.SessionId.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    IsEssential = true,
                    Expires = sess.AbsoluteExpiresAt
                });

                // -------------------------
                // 4. respuesta final
                // -------------------------
                return Ok(new LoginResultDto
                {
                    IsSuccess = true,
                    Message = "sesión iniciada"
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new LoginResultDto
                {
                    IsSuccess = false,
                    Message = "credenciales inválidas."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error en logindocumento");
                return StatusCode(500, new { isSuccess = false, message = "error interno." });
            }
        }


        // ===========================
        // Cerrar sesión (revoca y borra cookie)
        // ===========================
        [Authorize(AuthenticationSchemes = "DocSession")]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("ph_session", out var raw) && Guid.TryParse(raw, out var sid))
                await _svc.RevokeAsync(sid);

            Response.Cookies.Delete("ph_session", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });

            return Ok(new { isSuccess = true });
        }


    //    // ===========================
    //    // Validar token existente (tu endpoint actual JWT)
    //    // ===========================
    //    [HttpGet("ValidarToken")]
    //    [ProducesResponseType(typeof(object), 200)]
    //    public IActionResult ValidarToken([FromQuery] string token)
    //    {
    //        bool respuesta = _token.validarToken(token);
    //        return Ok(new { isSuccess = respuesta });
    //    }

    //    [Authorize(AuthenticationSchemes = "DocSession")]
    //    [HttpGet("ping")]
    //    public IActionResult Ping() => NoContent(); // 204
    }

}
