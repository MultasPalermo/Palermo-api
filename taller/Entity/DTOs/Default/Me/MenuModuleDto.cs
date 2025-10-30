using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.Me
{
    public class MenuModuleDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? icon { get; set; }

        public IEnumerable<FormMeDto> forms { get; set; } = [];
    }
}
