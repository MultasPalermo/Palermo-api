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
    public class ValueSmldvRepository : DataGeneric<ValueSmldv>, IValueSmldvRepository
    {
        public ValueSmldvRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<ValueSmldv>> GetAllAsync()
        {
            return await _dbSet
                .Where(v => v.is_deleted == false)
                .ToListAsync();
        }

        public override async Task<IEnumerable<ValueSmldv>> GetDeletes()
        {
            return await _dbSet
                .Where(v => v.is_deleted == true)
                .ToListAsync();
        }

        public override async Task<ValueSmldv?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Where(v => v.id == id)
                .FirstOrDefaultAsync();
        }
    }
}


