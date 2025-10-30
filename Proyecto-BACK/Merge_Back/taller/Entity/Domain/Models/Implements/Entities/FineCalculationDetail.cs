using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Entities;

public class FineCalculationDetail : BaseModel
{
    public string formula { get; set; }

    //public decimal porcentaje { get; set; }
    public decimal totalCalculation { get; set; }

    public int valueSmldvId { get; set; }
    public ValueSmldv valueSmldv { get; set; }

    public int typeInfractionId { get; set; }
    public Infraction Infraction { get; set; }
    public decimal SmldvValueAtCreation { get; set; }

}
