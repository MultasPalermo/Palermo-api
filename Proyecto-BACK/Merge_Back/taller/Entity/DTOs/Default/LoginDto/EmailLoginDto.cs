using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.LoginDto
{
    public class EmailLoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}
