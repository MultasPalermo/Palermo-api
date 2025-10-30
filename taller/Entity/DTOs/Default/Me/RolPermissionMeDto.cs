using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.Me
{
    public class RolPermissionMeDto
    {
        public int permissionId { get; set; }
        public string permissionName { get; set; }
        public FormMeDto form { get; set; }
    }
}
