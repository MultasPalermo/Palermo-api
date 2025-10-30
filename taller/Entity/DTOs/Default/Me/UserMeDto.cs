using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.Me
{
    public class UserMeDto
    {
        public int id { get; set; }
        public string fullName { get; set; } = null!;

        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string email { get; set; } = null!;

        public IEnumerable<string> roles { get; set; } = [];
        public IEnumerable<string> permissions { get; set; } = [];

        public IEnumerable<MenuModuleDto> Menu { get; set; } = [];

    }
}
