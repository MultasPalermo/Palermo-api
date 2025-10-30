using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IDataImplement.parameters;
using Data.Repositoy;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Domain.Models.Implements.parameters;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Entities
{
    public class municipalityRepository : DataGeneric<municipality>, ImunicipalityRepository
    {
        public municipalityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<municipality>> GetAllAsync()
        {
            return await _dbSet
                        .Include(d => d.department)
                        .Where(d => d.is_deleted == false)
                        .ToListAsync();
        }

        public override async Task<IEnumerable<municipality>> GetDeletes()
        {
            return await _dbSet
                        .Include(d => d.department)
                        .Where(d => d.is_deleted == true)
                        .ToListAsync();
        }

        public override async Task<municipality?> GetByIdAsync(int id)
        {
            return await _dbSet
                      .Include(d => d.department)
                      .Where(d => d.id == id)
                      .FirstOrDefaultAsync();

        }
    }
}
