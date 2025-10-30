using Data.Interfaces.IDataImplement.Security;
using Data.Repositoy;
using Entity.DTOs.Default.Auth.RestPasword;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Security
{
    public class PasswordResetCodeRepository : DataGeneric<PasswordResetCode>, IPasswordResetCodeRepository
    {
        public PasswordResetCodeRepository(ApplicationDbContext context) : base(context) { }

        public Task<PasswordResetCode?> GetValidCodeAsync(string email, string code)
            => _dbSet.FirstOrDefaultAsync(c =>
                c.email == email &&
                c.code == code &&
                !c.isUsed &&
                c.expiration > DateTime.UtcNow);
    }
}
