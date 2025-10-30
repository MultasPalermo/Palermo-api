using Entity.DTOs.Interface.ModelSecurity;

namespace Entity.DTOs.Select.ModelSecuritySelectDto
{
    public class FormModuleSelectDto : IFormModule
    {
        public int id { get; set; }
        public int formid { get; set; }
        public int moduleid { get; set; }
        public string formName { get; set; }
        public string moduleName { get; set; }
    }
}
