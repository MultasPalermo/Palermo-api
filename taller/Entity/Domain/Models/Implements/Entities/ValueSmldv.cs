using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class ValueSmldv : BaseModel
    {
        public decimal value_smldv { get; set; }   // 👈 decimal para pesos colombianos
        public int Current_Year { get; set; }
        public decimal minimunWage { get; set; }

        // Relaciones
        public ICollection<FineCalculationDetail> fineCalculationDetail { get; set; }
    }

}
