using Business.Interfaces.IBusinessImplements.Security;
using Data.Interfaces.IDataImplement.Security;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

public class AuthSessionService : IAuthSessionService
{
    private readonly IAuthSessionRepository _repo;
    private readonly ISystemClock _clock;
    private readonly TimeSpan _idle;
    private readonly TimeSpan _absolute;
    private readonly ILogger<AuthSessionService> _log;

    public AuthSessionService(
        IAuthSessionRepository repo,
        ISystemClock clock,
        IConfiguration cfg,
        ILogger<AuthSessionService> log)
    {
        _repo = repo;
        _clock = clock;
        _log = log;

        var idleMin = int.TryParse(cfg["Auth:IdleMinutes"], out var i) ? i : 15;
        var absMin = int.TryParse(cfg["Auth:AbsoluteMinutes"], out var a) ? a : 120;
        _log.LogInformation("Auth timeouts => idle:{Idle}m absolute:{Abs}m", idleMin, absMin);

        _idle = TimeSpan.FromMinutes(idleMin);
        _absolute = TimeSpan.FromMinutes(absMin);
    }

    public async Task<AuthSession> CreateSessionAsync(long? personId, string ip, string? ua)
    {
        var now = _clock.UtcNow.UtcDateTime;
        var s = new AuthSession
        {
            SessionId = Guid.NewGuid(), // ✅ usar Guid
            PersonId = personId,
            CreatedAt = now,
            LastActivityAt = now,
            AbsoluteExpiresAt = now.Add(_absolute),
            IsRevoked = false,
            Ip = ip,
            UserAgent = ua
        };

        await _repo.CreateAsync(s);
        return s;
    }

    public async Task<(bool ok, ClaimsPrincipal? user)> ValidateAsync(Guid sessionId) // ✅ Guid
    {
        var now = _clock.UtcNow.UtcDateTime;
        var s = await _repo.GetAsync(sessionId);

        if (s is null) { _log.LogWarning("No session {sid}", sessionId); return (false, null); }
        if (s.IsRevoked) { _log.LogInformation("Session {sid} revoked", sessionId); return (false, null); }

        if (now >= s.AbsoluteExpiresAt)
        {
            _log.LogInformation("Session {sid} absolute expired at {abs}", s.SessionId, s.AbsoluteExpiresAt);
            return (false, null);
        }

        var idle = now - s.LastActivityAt;
        if (idle > _idle)
        {
            _log.LogInformation("Session {sid} idle expired: idle {idle} > limit {_idle}", s.SessionId, idle, _idle);
            return (false, null);
        }

        _log.LogDebug("Session {sid} ok. idle {idle}/{_idle}. Touch()", s.SessionId, idle, _idle);
        await _repo.TouchAsync(sessionId, now);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, s.SessionId.ToString()),
            new Claim(ClaimTypes.Role, "ConsultaMultas")
        };

        if (s.PersonId.HasValue)
            claims.Add(new Claim("person_id", s.PersonId.Value.ToString()));

        var identity = new ClaimsIdentity(claims, "DocSession");
        return (true, new ClaimsPrincipal(identity));
    }

    public Task RevokeAsync(Guid sessionId) => _repo.RevokeAsync(sessionId); // ✅ Guid
}
