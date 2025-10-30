using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.LoginDto.response.RegisterReponseDto
{
    public class RegisterResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public double? RecaptchaScore { get; set; }

    }
}
