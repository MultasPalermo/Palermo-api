using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace Data.Interfaces.IDataImplement.Security
{
    public interface IPersonRepository : IData<Person>
    {
    }
}
