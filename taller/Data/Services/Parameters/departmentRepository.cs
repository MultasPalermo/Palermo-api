using Data.Interfaces.IDataImplement.parameters;
using Data.Repositoy;
using Entity.Domain.Models.Implements.parameters;
using Entity.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Parameters
{
    public class departmentRepository : DataGeneric<department>, IdepartmentRepository
    {
        public departmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
