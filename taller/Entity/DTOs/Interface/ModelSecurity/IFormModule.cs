using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;

namespace Entity.DTOs.Interface.ModelSecurity
{
    public interface IFormModule : IHasId
    {
        public int id { get; set; }
        public int formid { get; set; }
        public int moduleid { get; set; }
    }
}
