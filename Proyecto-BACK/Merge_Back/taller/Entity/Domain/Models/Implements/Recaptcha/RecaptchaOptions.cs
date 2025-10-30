using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Models.Implements.Recaptcha
{
    public class RecaptchaOptions
    {
        public string SecretKey { get; set; } = "";
        public double MinScore { get; set; } = 0.5;
    }
}
