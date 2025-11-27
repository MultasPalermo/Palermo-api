using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
   public class JwtSettings
    {
        public string key { get; set; } = string.Empty;
        public string Issuer { get; set; } = "PalermoAPI";
        public string Audience { get; set; } = "PalermoClients";
        public int AccessTokenExpirationMinutes { get; set; } = 60;
        public int RefreshTokenExpirationDays { get; set; } = 7;
        public int ExpireMinutes { get; set; } = 60;
   }
}
