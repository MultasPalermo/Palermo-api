namespace Entity.Domain.Models.Implements.Entities
{
    public class FineCalculationDetailSelectDto 
    {
        public int id { get; set; }
        public string formula { get; set; }
        //public decimal porcentaje { get; set; }
        public decimal totalCalculation { get; set; }

        public int valueSmldvId { get; set; }
        public double valueSmldvValue { get; set; }
        public int currentYear { get; set; }
        public decimal minimunWage { get; set; }

        public int typeInfractionId { get; set; }
        public string TypeInfractionName { get; set; }
        public int numerSmldv { get; set; }
        public string description { get; set; }
        public decimal SmldvValueAtCreation { get; set; }


    }
}
