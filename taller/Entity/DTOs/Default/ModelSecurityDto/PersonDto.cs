using System.ComponentModel.DataAnnotations;
using Entity.Domain.Interfaces;
using Entity.DTOs.Interface.ModelSecurity;

namespace Entity.DTOs.Default.ModelSecurityDto
{
    public class PersonDto : IHasId, IPerson
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? phoneNumber { get; set; }
        public string? address { get; set; }
        public int municipalityId { get; set; }
    }
}
