using Azure.Core;
using Business.Interfaces.IBusinessImplements.Security;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Web.Infrastructure
{
    //public class DocSessionAuthenticationHandler : AuthenticationHandler<DocSessionAuthenticationOptions>
    //{
    //    private readonly IAuthSessionService _svc;
    //    public DocSessionAuthenticationHandler(
    //        IOptionsMonitor<DocSessionAuthenticationOptions> options,
    //        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAuthSessionService svc)
    //        : base(options, logger, encoder, clock) { _svc = svc; }

    //    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    //    {
    //        if (!Request.Cookies.TryGetValue(Options.CookieName, out var raw))
    //            return AuthenticateResult.NoResult();

    //        if (!Guid.TryParse(raw, out var sid))
    //            return AuthenticateResult.Fail("Invalid session id");

    //        var (ok, principal) = await _svc.ValidateAsync(sid);
    //        if (!ok || principal is null)
    //            return AuthenticateResult.Fail("Session expired");

    //        var ticket = new AuthenticationTicket(principal, Scheme.Name);
    //        return AuthenticateResult.Success(ticket);
    //    }
    //}
}
