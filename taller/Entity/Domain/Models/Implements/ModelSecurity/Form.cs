using Entity.Domain.Models.Base;
using Entity.DTOs.Interface.ModelSecurity;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
    public class Form : BaseModelGeneric , IForm
    {
        // Relaciones

        public string? Route { get; set; } = null!;
        public string? Icon { get; set; } = null!;

        public List<RolFormPermission> rol_form_permission { get; set; } = new List<RolFormPermission>();

        public List<FormModule> FormModules { get; set; } = new List<FormModule>();
    }
}
