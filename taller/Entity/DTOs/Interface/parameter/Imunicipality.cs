using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;

namespace Entity.DTOs.Interface.parameter
{
    public interface Imunicipality : IHasId
    {
        public int id { get; set; }
        public string name { get; set; }
        public int daneCode { get; set; }
        public int departmentId { get; set; }
    }
}
