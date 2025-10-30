using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Entities;

namespace Entity.Domain.Models.Implements.parameters
{
    public class PaymentFrequencySelectDto 
    {
        public int id { get; set; }
        public string intervalPage { get; set; }
        public int dueDayOfMonth {get; set; }
    }
}
