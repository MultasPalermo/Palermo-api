using Entity.Domain.Interfaces;

namespace Entity.DTOs.Default.ModelSecurityDto
{
    public class RolUserDto : IHasId
    {

        public int id { get; set; }
        public int userId { get; set;}
        public int rolId {  get; set;}
    }
}
