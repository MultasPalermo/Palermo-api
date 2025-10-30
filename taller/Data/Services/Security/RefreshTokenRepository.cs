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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _ctx;

        public RefreshTokenRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task AddAsync(RefreshToken token)
        {
            _ctx.Set<RefreshToken>().Add(token);
            await _ctx.SaveChangesAsync();
        }

        public Task<RefreshToken?> GetByHashAsync(string tokenHash)
        {
            return _ctx.Set<RefreshToken>()
                       .AsNoTracking()
                       .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);
        }

        public async Task RevokeAsync(RefreshToken token, string? replacedByTokenHash = null)
        {
            var tracked = await _ctx.Set<RefreshToken>().FirstOrDefaultAsync(t => t.Id == token.Id);
            if (tracked == null) return;

            tracked.IsRevoked = true;
            tracked.ReplacedByTokenHash = replacedByTokenHash;
            await _ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<RefreshToken>> GetValidTokensByUserAsync(int userId)
        {
            var now = DateTime.UtcNow;
            return await _ctx.Set<RefreshToken>()
                             .Where(t => t.UserId == userId && !t.IsRevoked && t.ExpiresAt > now)
                             .AsNoTracking()
                             .OrderByDescending(t => t.CreatedAt)
                             .ToListAsync();
        }
    }
}
