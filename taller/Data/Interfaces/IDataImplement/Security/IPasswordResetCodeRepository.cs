using Data.Interfaces.DataBasic;
using Data.Repositoy;
using Entity.DTOs.Default.Auth.RestPasword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.IDataImplement.Security
{
    public interface IPasswordResetCodeRepository : IData<PasswordResetCode>
    {
        //Task AddAsync(PasswordResetCode resetCode);
        Task<PasswordResetCode?> GetValidCodeAsync(string email, string code);
    }
}
