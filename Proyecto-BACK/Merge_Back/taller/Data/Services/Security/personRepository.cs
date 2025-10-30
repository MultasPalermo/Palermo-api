using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.IDataImplement;
using Data.Interfaces.IDataImplement.Security;
using Data.Repositoy;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Services.Security
{
    public class personRepository : DataGeneric<Person>, IPersonRepository
    {
        public personRepository(ApplicationDbContext context) : base(context) 
        {
        }

        public override async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _dbSet
                        .Include(p => p.municipality)
                        .Where(p => p.is_deleted == false)
                        .ToListAsync();
        }
        public override async Task<IEnumerable<Person>> GetDeletes()
        {
            return await _dbSet
                        .Include(p => p.municipality)
                        .Where(p => p.is_deleted == true)
                        .ToListAsync();
        }
        public override async Task<Person> GetByIdAsync(int id)
        {
            return await _dbSet
                      .Include(p => p.municipality)
                      .Where(p => p.id == id)
                      .FirstOrDefaultAsync();

        }
    }
}
