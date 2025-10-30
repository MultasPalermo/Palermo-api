using Data.Interfaces.IDataImplement.Security;
using Data.Repositoy;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Security
{
    public class RolUserRepository : DataGeneric<RolUser>, IRolUserRepository
    {
        public RolUserRepository(ApplicationDbContext context) :    base(context)
        {
        }

        public async Task<RolUser> AsignateUserRTo(User user)
        {
            var rolUser = new RolUser
            {
                UserId = user.id,
                RolId = 2,
                active = true,
                is_deleted = false
            };

            _context.rolUsers.Add(rolUser);
            await _context.SaveChangesAsync();

            return rolUser;
        }


        public override async Task<IEnumerable<RolUser>> GetAllAsync()
        {
            return await _dbSet
                        .Include(u => u.Rol)
                        .Include(u => u.User)
                        .Where(u => u.is_deleted == false)
                        .ToListAsync();
        }

        public override async Task<IEnumerable<RolUser>> GetDeletes()
        {
            return await _dbSet
                        .Include(u => u.Rol)
                        .Include(u => u.User)
                        .Where(u => u.is_deleted == true)
                        .ToListAsync();
        }


        public override async Task<RolUser?> GetByIdAsync(int id)
        {
            return await _dbSet
                      .Include(u => u.Rol)
                      .Include(u => u.User)
                      .Where(u => u.id == id)
                        .Where(u => u.is_deleted == false)

                      .FirstOrDefaultAsync();   

        }

        public async Task<IEnumerable<string>> GetJoinRolesAsync(int idUser)
        {
            var rolAsignated = await _dbSet
                               .Include(ru => ru.Rol)
                               .Include(ru => ru.User)
                               .Where(ru => ru.UserId == idUser)
                               .ToListAsync();

            var roles = rolAsignated
                                .Select(ru => ru.Rol.name)
                                .Where(name => !string.IsNullOrWhiteSpace(name))
                                .Distinct()
                                .ToList();
            return roles;
        }

        public Task<List<string>> GetRoleNamesByUserIdAsync(int userId)
    => GetJoinRolesAsync(userId).ContinueWith(t => t.Result.ToList());

    }
}
