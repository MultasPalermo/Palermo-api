using Entity.Domain.Interfaces;
using Entity.DTOs.Interface.ModelSecurity;

namespace Entity.DTOs.Default.ModelSecurityDto
{
    public class FormModuleDto : IHasId, IFormModule
    {
        public int id { get; set; }
        public int formid { get; set; }
        public int moduleid { get; set; }
    }
}
