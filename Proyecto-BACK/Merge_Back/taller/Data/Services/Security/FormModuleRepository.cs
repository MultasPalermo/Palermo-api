using Data.Interfaces.IDataImplement.Security;
using Data.Repositoy;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Security
{
    public class FormModuleRepository : DataGeneric<FormModule>, IFormModuleRepository
    {
        public FormModuleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<FormModule>> GetAllAsync()
        {
            return await _dbSet
                        .Include(u => u.form)
                        .Include(u => u.module)
                        .Where(u => u.is_deleted == false)
                        .ToListAsync();
        }

        public override async Task<IEnumerable<FormModule>> GetDeletes()
        {
            return await _dbSet
                        .Include(u => u.form)
                        .Include(u => u.module)
                        .Where(u => u.is_deleted == true)
                        .ToListAsync();
        }

        public override async Task<FormModule?> GetByIdAsync(int id)
        {
            return await _dbSet
                      .Include(u => u.form)
                      .Include(u => u.module)
                      .Where(u => u.id == id)
                      .FirstOrDefaultAsync();

        }
    }
}
