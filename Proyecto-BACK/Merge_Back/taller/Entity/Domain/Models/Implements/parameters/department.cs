using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;

namespace Entity.Domain.Models.Implements.parameters
{
    public class department : BaseModel
    {
        public string name { get; set; }
        public int daneCode { get; set; }

        public ICollection<municipality> Municipality { get; set; } = new List<municipality>();
    }
}
