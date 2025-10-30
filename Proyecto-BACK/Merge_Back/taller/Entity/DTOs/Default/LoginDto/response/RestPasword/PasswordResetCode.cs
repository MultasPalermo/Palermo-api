using Entity.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.Auth.RestPasword
{
    public class PasswordResetCode : BaseModel
    {
        public string email { get; set; } = null!;
        public string code { get; set; } = null!;
        public DateTime expiration { get; set; }
        public bool isUsed { get; set; } = false;
    }
}
