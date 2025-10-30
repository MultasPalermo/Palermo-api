using Data.Interfaces.IDataImplement.Security;
using Data.Repositoy;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Security
{
    public class RolFormPermissionRepository : DataGeneric<RolFormPermission>, IRolFormPermissionRepository
    {
        public RolFormPermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<RolFormPermission>> GetAllAsync()
        {
            return await _dbSet
                        .Include(u => u.Rol)
                        .Include(u => u.Form)
                        .Include(u => u.Permission)
                        .Where(u => u.is_deleted == false)

                        .ToListAsync();
        }

        public override async Task<IEnumerable<RolFormPermission>> GetDeletes()
        {
            return await _dbSet
                        .Include(u => u.Rol)
                        .Include(u => u.Form)
                        .Include(u => u.Permission)
                        .Where(u => u.is_deleted == true)

                        .ToListAsync();
        }

        public override async Task<RolFormPermission?> GetByIdAsync(int id)
        {
            return await _dbSet
                      .Include(u => u.Rol)
                      .Include(u => u.Form)
                      .Include(u => u.Permission)
                      .Where(u => u.id == id)
                      .FirstOrDefaultAsync();

        }
    }
}
