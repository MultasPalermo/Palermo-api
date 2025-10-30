using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.DTOs.Interface.parameter;

namespace Entity.Domain.Models.Implements.parameters
{
    public class departmentSelectDto : Idepartment
    {
        public int id { get; set;  }
        public string name { get; set; }
        public int daneCode { get; set; }
    }
}
