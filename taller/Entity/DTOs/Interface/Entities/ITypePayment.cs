using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;

namespace Entity.DTOs.Interface.Entities
{
    public interface ITypePayment : IHasId
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
