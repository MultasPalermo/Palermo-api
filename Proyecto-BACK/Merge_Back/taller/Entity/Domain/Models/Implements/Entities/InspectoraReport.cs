using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class InspectoraReport : BaseModel
    {
        public DateTime report_date { get; set; }
        public decimal total_fines { get; set; }
        public string message { get; set; }

        //relaciones
        public List<DocumentInfraction> documentInfraction { get; set; } = new List<DocumentInfraction>();
    }
}
