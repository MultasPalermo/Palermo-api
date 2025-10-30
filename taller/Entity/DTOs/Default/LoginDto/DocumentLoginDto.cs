using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.LoginDto
{
    public class DocumentLoginDto
    {
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; } = null!;

        public string RecaptchaToken { get; set; } 
        public string RecaptchaAction { get; set; } 

    }
}
