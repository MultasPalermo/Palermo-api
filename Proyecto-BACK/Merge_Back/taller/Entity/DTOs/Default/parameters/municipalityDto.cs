using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;
using Entity.DTOs.Interface.parameter;

namespace Entity.DTOs.Default.parameters
{
    public class municipalityDto : IHasId, Imunicipality
    {
       public int id { get; set; }
       public string name { get; set; }
       public int daneCode { get; set; }
       public int departmentId { get; set; }
    }
}
