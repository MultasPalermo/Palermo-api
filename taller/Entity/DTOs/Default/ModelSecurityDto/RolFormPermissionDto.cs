using Entity.Domain.Interfaces;

namespace Entity.DTOs.Default.ModelSecurityDto
{
    public class RolFormPermissionDto : IHasId
    {
        public int id { get; set; }
        public int rolid { get; set; }
        public int formid { get; set; }
        public int permissionid { get; set; }
    }
}
