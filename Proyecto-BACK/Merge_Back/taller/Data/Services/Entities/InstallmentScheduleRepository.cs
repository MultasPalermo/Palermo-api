using Data.Interfaces.IDataImplement.Entities;
using Data.Repositoy;
using Entity.Domain.Models.Implements.Entities;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Entities
{
    public class InstallmentScheduleRepository : DataGeneric<InstallmentSchedule>, IInstallmentScheduleRepository
    {
        public InstallmentScheduleRepository(ApplicationDbContext context) : base(context) { }
    }
}
