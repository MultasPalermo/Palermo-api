using Business.Mensajeria.Email.implements;
using Business.Mensajeria.Email.@interface;
using Entity.DTOs.Default.Email;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implements
{
    [ApiController]
    [Route("api/verificacion")]
    public class VerificationController : ControllerBase
    {
        private readonly IVerificationService _verificationService;

        public VerificationController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendCode([FromBody] SendVerificationDto dto)
        {
            await _verificationService.SendVerificationAsync(dto.Nombre, dto.Email);
            return Ok(new { message = "Código enviado al correo" });
        }

        [HttpPost("validate")]
        public IActionResult ValidateCode([FromBody] VerificationRequestDto dto)
        {
            var result = _verificationService.ValidateCode(dto.Email, dto.Code);
            return result ? Ok(new { valid = true }) : BadRequest(new { valid = false });
        }
    }
}

