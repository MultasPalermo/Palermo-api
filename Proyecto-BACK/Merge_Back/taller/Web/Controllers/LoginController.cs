using Business.ExternalServices.Recaptcha;
using Business.Interfaces.IBusinessImplements.Security;
using Business.Interfaces.IJWT;
using Entity.DTOs.Default.Email;
using Entity.DTOs.Default.LoginDto.response.LoginResultDto;
using Entity.DTOs.Default.LoginDto.response.RegisterReponseDto;
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
        private readonly IRecaptchaVerifier _recaptcha;
        private readonly IAuthSessionService _svc;   // ADD: servicio de sesiones
        private readonly ISystemClock _clock;              // ADD: reloj (tu wrapper)

        //private readonly IServiceEmail _serviceEmail;
        //private readonly INotifyManager _notifyManager;

        public LoginController(
            EncriptePassword utilities,
            IToken token,
            IRecaptchaVerifier recaptcha,
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
            _recaptcha = recaptcha;
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
        public async Task<IActionResult> LoginDocumento([FromBody] DocumentLoginDto login)
        {
            try
            {
                // 1) Verificar reCAPTCHA v3
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                var (ok, reason, score) = await _recaptcha.VerifyAsync(
                    login.RecaptchaToken,
                    login.RecaptchaAction ?? "documento",
                    ip
                );

                if (!ok)
                {
                    // BadRequest con el mismo tipo de respuesta
                    return BadRequest(new LoginResultDto
                    {
                        IsSuccess = false,
                        Message = $"reCAPTCHA inválido: {reason} (score: {score:F2})"
                    });
                }

                // (Opcional) Aplica umbral local de score si quieres ser más estricto
                // if (score < 0.5) return BadRequest(new LoginResultDto { IsSuccess = false, Message = "Acceso bloqueado por sospecha de bot." });

                // 2) (Opcional) Validar existencia/obtención de persona
                long? personId = null;
                // personId = await _userService.GetPersonIdByDocAsync(login.DocumentTypeId, login.DocumentNumber);

                // 3) Crear sesión (Business) y setear cookie HttpOnly
                var ua = Request.Headers.UserAgent.ToString();
                var sess = await _svc.CreateSessionAsync(personId, ip ?? "-", ua); // _svc: tu servicio de sesiones

                Response.Cookies.Append("ph_session", sess.SessionId.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,                 // PRODUCCIÓN: true (requiere HTTPS). En dev puedes poner false.
                    SameSite = SameSiteMode.None,  // Si el front está en otro origen/puerto; si es mismo origen, Lax está bien.
                    IsEssential = true,
                    Expires = sess.AbsoluteExpiresAt.Date
                });

                // 4) Responder éxito (sin JWT)
                return Ok(new LoginResultDto
                {
                    IsSuccess = true,                 // no usamos JWT en este flujo
                    Message = "Sesión iniciada"
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new LoginResultDto
                {
                    IsSuccess = false,
                    Message = "Credenciales inválidas."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en LoginDocumento");
                return StatusCode(500, new { isSuccess = false, message = "Error interno." });
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
