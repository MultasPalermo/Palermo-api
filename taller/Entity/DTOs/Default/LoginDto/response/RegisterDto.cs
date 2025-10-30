using Entity.Domain.Models.Implements.parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.Auth
{
   public  class RegisterDto
    {
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
  
    }
}
