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
    public class InfractionRepository : DataGeneric<Infraction>, IInfractionRepository
    {
        public InfractionRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Obtener todos los registros activos
        public override async Task<IEnumerable<Infraction>> GetAllAsync()
        {
            return await _dbSet
                .Include(i => i.TypeInfraction) // 🔹 Incluir relación
                .Where(t => t.is_deleted == false)
                .ToListAsync();
        }

        // Obtener registros eliminados (lógica de borrado)
        public override async Task<IEnumerable<Infraction>> GetDeletes()
        {
            return await _dbSet
                .Include(i => i.TypeInfraction) // 🔹 Incluir relación
                .Where(t => t.is_deleted == true)
                .ToListAsync();
        }

        // Obtener un registro por ID
        public override async Task<Infraction?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(i => i.TypeInfraction) // 🔹 Incluir relación
                .Where(t => t.id == id)
                .FirstOrDefaultAsync();
        }
    }
}
