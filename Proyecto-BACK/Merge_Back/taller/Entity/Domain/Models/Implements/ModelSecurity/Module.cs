using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
    public class Module : BaseModelGeneric
    {
      
        // Relación con FormModules
        public  List<FormModule> FormModules { get; set; } = new List<FormModule>();
    }
}
