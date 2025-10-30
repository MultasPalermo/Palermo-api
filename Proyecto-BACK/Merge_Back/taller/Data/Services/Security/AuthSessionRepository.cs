// Data/Services/Security/AuthSessionRepository.cs
using Data.Interfaces.IDataImplement.Security;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Infrastructure.Contexts;          // ApplicationDbContext
using Microsoft.EntityFrameworkCore;

public class AuthSessionRepository : IAuthSessionRepository
{
    private readonly ApplicationDbContext _ctx;
    public AuthSessionRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task CreateAsync(AuthSession s)
    {
        _ctx.Set<AuthSession>().Add(s);
        await _ctx.SaveChangesAsync();
    }

    public Task<AuthSession?> GetAsync(Guid id) =>
        _ctx.Set<AuthSession>().FirstOrDefaultAsync(x => x.SessionId == id);

    public async Task TouchAsync(Guid id, DateTime now)
    {
        var s = await GetAsync(id);
        if (s is null) return;
        s.LastActivityAt = now;
        await _ctx.SaveChangesAsync();
    }

    public async Task RevokeAsync(Guid id)
    {
        var s = await GetAsync(id);
        if (s is null) return;
        s.IsRevoked = true;
        await _ctx.SaveChangesAsync();
    }
}
