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
    public class FineCalculationDetailsRepository : DataGeneric<FineCalculationDetail>, IFineCalculationDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public FineCalculationDetailsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<FineCalculationDetail>> GetAllAsync()
        {
            return await _dbSet
                .Include(f => f.valueSmldv)
                .Include(f => f.Infraction)
                    .ThenInclude(i => i.TypeInfraction)
                .Where(f => !f.is_deleted)
                .ToListAsync();
        }

        public override async Task<IEnumerable<FineCalculationDetail>> GetDeletes()
        {
            return await _dbSet
                .Include(f => f.valueSmldv)
                .Include(f => f.Infraction)
                .ThenInclude(i => i.TypeInfraction)
                .Where(f => f.is_deleted)
                .ToListAsync();
        }

        public override async Task<FineCalculationDetail?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(f => f.valueSmldv)
                .Include(f => f.Infraction)
                .ThenInclude(i => i.TypeInfraction)
                .FirstOrDefaultAsync(f => f.id == id);
        }
    }
}

