using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
    public class FormModule : BaseModel
    {
       
        public int formid { get; set; }
        public int moduleid { get; set; }

        // Relaciones de navegación
        public Form form { get; set; }
        public Module module { get; set; }
    }
}
