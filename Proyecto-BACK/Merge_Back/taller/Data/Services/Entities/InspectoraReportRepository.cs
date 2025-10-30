using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IDataImplement.Entities;
using Data.Repositoy;
using Entity.Domain.Models.Implements.Entities;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Entities
{
    public class InspectoraReportRepository : DataGeneric<InspectoraReport>, IInspectoraReportRepository
    {
        public InspectoraReportRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<InspectoraReport?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.documentInfraction).ThenInclude(x => x.paymentAgreement)
                               .FirstOrDefaultAsync(x => x.id == id);
        }
    }
}
