using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.DTOs.Interface.parameter;

namespace Entity.Domain.Models.Implements.parameters
{
    public class municipalitySelectDto : Imunicipality
    {
        public int id { get; set; }
        public string name { get; set; }
        public int daneCode { get; set; }
        public  int departmentId { get; set; }
        public string departmentName { get; set; }

    }
}
