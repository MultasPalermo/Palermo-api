using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;

namespace Entity.DTOs.Interface.ModelSecurity
{
    public interface IPerson : IHasId
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? phoneNumber { get; set; }
        public string? address { get; set; }
        public int municipalityId { get; set; }
    }
}
