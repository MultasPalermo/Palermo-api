using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace Entity.Domain.Models.Implements.parameters
{
    public class documentType : BaseModel
    {
        public string name { get; set; }
        public string abbreviation { get; set; }

        //relacion

        public ICollection<User> person { get; set; } = new List<User>();
    }
}
