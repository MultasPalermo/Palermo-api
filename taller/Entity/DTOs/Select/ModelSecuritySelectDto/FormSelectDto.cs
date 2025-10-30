using Entity.DTOs.Interface.ModelSecurity;

namespace Entity.DTOs.Select.ModelSecuritySelectDto
{
    public class FormSelectDto : IForm
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
