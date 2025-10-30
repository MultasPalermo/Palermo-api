using Data.Interfaces.Security;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Security
{
    public class MeRepository : IUserMeRepository
    {
        private readonly ApplicationDbContext _context;
        public MeRepository(ApplicationDbContext context) => _context = context;

        public async Task<User?> GetUserWithPersonAsync(int userId)
        {
            return await _context.users
                .Include(u => u.Person)                
                .FirstOrDefaultAsync(u => u.id == userId && u.is_deleted == false);
        }

        public async Task<User?> GetUserWithFullContextAsync(int userId)
        {
            return await _context.users
                .AsNoTracking()
                .Where(u => u.id == userId)
                .Include(u => u.Person)                 
                .Include(u => u.rolUsers)               
                    .ThenInclude(ru => ru.Rol)          
                        .ThenInclude(r => r.rol_form_permission)        
                            .ThenInclude(rfp => rfp.Form)               
                                .ThenInclude(f => f.FormModules)        
                                    .ThenInclude(fm => fm.module)       
                .Include(u => u.rolUsers)
                    .ThenInclude(ru => ru.Rol)
                        .ThenInclude(r => r.rol_form_permission)
                            .ThenInclude(rfp => rfp.Permission)        
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<RolUser>> GetUserRolesWithPermissionsAsync(int userId)
        {
            return await _context.rolUsers
                .Include(ur => ur.Rol)
                    .ThenInclude(r => r.rol_form_permission)
                        .ThenInclude(rfp => rfp.Permission)
                .Include(ur => ur.Rol)
                    .ThenInclude(r => r.rol_form_permission)
                        .ThenInclude(rfp => rfp.Form)
                .Where(ur => ur.UserId == userId && ur.is_deleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Form>> GetFormsWithModulesByIdsAsync(List<int> formIds)
        {
            return await _context.forms
                .Include(f => f.FormModules)
                    .ThenInclude(mf => mf.module)
                .Where(f => formIds.Contains(f.id) && f.is_deleted == false) 
                .ToListAsync();
        }
    }


}

  
