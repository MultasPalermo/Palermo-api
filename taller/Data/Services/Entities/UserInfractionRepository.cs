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
    public class UserInfractionRepository : DataGeneric<UserInfraction>, IUserInfractionRepository
    {
        public UserInfractionRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<UserInfraction>> GetAllAsync()
        {
            return await _dbSet
                .Include(u => u.Infraction)
                    .ThenInclude(i => i.TypeInfraction) // 🔹 incluir TypeInfraction
                .Include(u => u.User)
                    .ThenInclude(ui => ui.Person)
                .Where(u => !u.is_deleted)
                .ToListAsync();
        }

        public override async Task<IEnumerable<UserInfraction>> GetDeletes()
        {
            return await _dbSet
                .Include(u => u.Infraction)
                    .ThenInclude(i => i.TypeInfraction)
                .Include(u => u.User)
                    .ThenInclude(ui => ui.Person)
                .Where(u => u.is_deleted)
                .ToListAsync();
        }

        public override async Task<UserInfraction?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Infraction)
                    .ThenInclude(i => i.TypeInfraction)
                .Include(u => u.User)
                    .ThenInclude(ui => ui.Person)
                .FirstOrDefaultAsync(u => u.id == id);
        }

        public async Task<IEnumerable<UserInfraction>> GetByDocumentAsync(int documentTypeId, string documentNumber)
        {
            documentNumber = documentNumber.Trim();

            return await _dbSet
                .AsNoTracking()
                .Include(u => u.Infraction)
                    .ThenInclude(i => i.TypeInfraction)
                .Include(u => u.User)
                    .ThenInclude(ui => ui.Person)
                .Where(u => !u.is_deleted &&
                            u.User.documentTypeId == documentTypeId &&
                            u.User.documentNumber == documentNumber)
                .ToListAsync();
        }

        public async Task<UserInfraction?> GetUserInfractionWithUserAndPersonAsync(int infractionId)
        {
            return await _context.userInfraction
                .Include(ui => ui.User)
                    .ThenInclude(u => u.Person)
                .Include(ui => ui.Infraction)
                    .ThenInclude(i => i.TypeInfraction)
                .FirstOrDefaultAsync(ui => ui.id == infractionId);
        }

        public async Task<IEnumerable<UserInfraction>> GetByTypeInfractionAsync(int typeInfractionId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(u => u.Infraction)
                    .ThenInclude(i => i.TypeInfraction)
                .Include(u => u.User)
                    .ThenInclude(ui => ui.Person)
                .Where(u => !u.is_deleted &&
                            u.Infraction.TypeInfractionId == typeInfractionId)
                .ToListAsync();
        }


    }
}
