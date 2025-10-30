using Entity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.EntitiesDto
{
    public class TypeInfractionDto : IHasId
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
}
