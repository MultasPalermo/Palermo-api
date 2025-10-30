using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class Infraction : BaseModel
    {
        public int TypeInfractionId { get; set; }
        public TypeInfraction TypeInfraction { get; set; }
        public string description { get; set; }
        public int numer_smldv { get; set; }

        //relaciones
        public List<UserInfraction> userInfractions { get; set; } = new List<UserInfraction>();
        public ICollection<FineCalculationDetail> fineCalculationDetail { get; set; }
    }
}
