using Entity.Domain.Interfaces;
using Entity.DTOs.Interface.ModelSecurity;

namespace Entity.DTOs.Default.ModelSecurityDto
{
    public class FormDto : IHasId , IForm
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
