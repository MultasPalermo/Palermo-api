using Entity.DTOs.Interface.ModelSecurity;

namespace Entity.DTOs.Select.ModelSecuritySelectDto
{
    public class PersonSelectDto : IPerson
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public int municipalityId { get; set; }
        public string municipalityName { get; set; }
    }
}
