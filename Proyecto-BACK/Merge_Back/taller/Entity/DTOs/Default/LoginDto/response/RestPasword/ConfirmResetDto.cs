using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.Auth.RestPasword
{
    public class ConfirmResetDto
    {
        public string email { get; set; } = null!;
        public string code { get; set; } = null!;
        public string newPassword { get; set; } = null!;
    }
}
