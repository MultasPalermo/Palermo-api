using Entity.Domain.Models.Base;
using System.Collections.Generic;

namespace Entity.Domain.Models.Implements.Entities
{
    public class TypeInfraction : BaseModel
    {
        public string Name { get; set; }  

        // Relación con Infraction (uno a muchos)
        public ICollection<Infraction> Infractions { get; set; } = new List<Infraction>();
    }
}
