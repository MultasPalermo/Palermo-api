using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
    public class Permission : BaseModelGeneric
    {


        // Relación con RolFormPermission
        public virtual ICollection<RolFormPermission> rol_form_permission { get; set; } = new List<RolFormPermission>();
    }
}
