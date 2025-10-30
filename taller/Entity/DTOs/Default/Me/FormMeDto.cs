using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.Me
{
   public class FormMeDto
    {
        public int id { get; set; }
        public string name { get; set; } 
        public string description { get; set; } 
        public string route { get; set; } 
        public bool state { get; set; } = true;

        public IEnumerable<string> Permissions { get; set; } = [];
    }
}
