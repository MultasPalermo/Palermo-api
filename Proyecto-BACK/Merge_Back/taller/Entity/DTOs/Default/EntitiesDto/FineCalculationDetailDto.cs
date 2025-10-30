using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class FineCalculationDetailDto : IHasId
    {
        public int id { get; set; }
        //public decimal percentaje { get; set; }
        public string formula {  get; set; }
        public decimal percentaje { get; set; }
        public decimal totalCalculation {  get; set; }
        public int valueSmldvId { get; set; }
        public string valueSmldvName { get; set; }
        public int typeInfractionId { get; set; }
        public string type_Infraction { get; set; }
        public decimal SmldvValueAtCreation { get; set; }


    }
}
