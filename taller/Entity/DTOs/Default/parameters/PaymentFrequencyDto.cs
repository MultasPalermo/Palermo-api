using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;

namespace Entity.DTOs.Default.parameters
{
    public class PaymentFrequencyDto : IHasId
    {
        public int id { get; set; }
        public string intervalPage { get; set; }
        public int dueDayOfMonth { get; set; }
    }
}
