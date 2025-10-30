using Entity.Domain.Models.Implements.ModelSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.Security
{
   public  interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetByHashAsync(string tokenHash);
        Task RevokeAsync(RefreshToken token, string? replacedByTokenHash = null);
        Task<IEnumerable<RefreshToken>> GetValidTokensByUserAsync(int userId);
    }
}
