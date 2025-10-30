using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;

namespace Entity.DTOs.Default.parameters
{
    public class documentTypeDto : IHasId
    {
        public int id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
    }
}
