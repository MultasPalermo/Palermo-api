using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.Entities
{
    public class InfractionSelectDto
    {
        public int id { get; set; }
        public string TypeInfractionName { get; set; }
        public int numer_smldv { get; set; }
        public string description { get; set; }
        
    }
}
