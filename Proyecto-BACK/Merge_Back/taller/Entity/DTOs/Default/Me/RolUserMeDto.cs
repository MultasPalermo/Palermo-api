using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.Me
{
    public class RolUserMeDto
    {
        public int rolId { get; set; }
        public string rolName { get; set; } = null!;

        public List<RolPermissionMeDto> permissions { get; set; } 
    }
}
