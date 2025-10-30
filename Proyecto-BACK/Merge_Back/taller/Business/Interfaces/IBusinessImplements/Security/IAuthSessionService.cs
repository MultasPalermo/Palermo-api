using Entity.Domain.Models.Implements.ModelSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.IBusinessImplements.Security
{
    public interface IAuthSessionService
    {
        Task<AuthSession> CreateSessionAsync(long? personId, string ip, string? userAgent);
        Task<(bool ok, ClaimsPrincipal? user)> ValidateAsync(Guid sessionId);
        Task RevokeAsync(Guid sessionId);
    }

}
