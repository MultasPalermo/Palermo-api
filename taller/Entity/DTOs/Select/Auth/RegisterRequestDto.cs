using Entity.DTOs.Default.ModelSecurityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.RegisterRequestDto
{
    public class RegisterRequestDto
    {
        public string NombreCompleto { get; set; }     // nombre completo en un solo campo
        public string email { get; set; }    // si quieres SOLO Gmail, valida @gmail.com
        public string password { get; set; }
    }

}
