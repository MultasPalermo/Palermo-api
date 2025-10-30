using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace Entity.Domain.Models.Implements.parameters
{
    public class municipality : BaseModel
    {
        public string name { get; set; }
        public int daneCode { get; set; }
        public int departmentId { get; set; }

        //relaciones

        public ICollection<Person> person { get; set; } = new List<Person>();
        public department department { get; set; }
    }
}
