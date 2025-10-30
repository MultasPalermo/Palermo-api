using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
   public class JwtSettings
    {
        public string key { get; set; } = null!;
        public string Issuer { get; set; } = "WebCDCP.API";
        public string Audience { get; set; } = "WebCDCP.Client";
        public int AccessTokenExpirationMinutes { get; set; } = 15;
        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}
