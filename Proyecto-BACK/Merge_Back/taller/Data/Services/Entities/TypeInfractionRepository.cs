using Data.Interfaces.IDataImplement.Entities;
using Data.Repositoy;
using Entity.Domain.Models.Implements.Entities;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Services.Entities
{
    public class TypeInfractionRepository : DataGeneric<TypeInfraction>, ITypeInfractionRepository
    {
        public TypeInfractionRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<TypeInfraction>> GetAllAsync()
        {
            return await _dbSet
                        .Include(t => t.Infractions)
                        .Where(t => !t.is_deleted)
                        .ToListAsync();
        }

        public override async Task<IEnumerable<TypeInfraction>> GetDeletes()
        {
            return await _dbSet
                        .Include(t => t.Infractions)
                        .Where(t => t.is_deleted)
                        .ToListAsync();
        }

        public override async Task<TypeInfraction?> GetByIdAsync(int id)
        {
            return await _dbSet
                        .Include(t => t.Infractions)
                        .FirstOrDefaultAsync(t => t.id == id);
        }
    }
}
