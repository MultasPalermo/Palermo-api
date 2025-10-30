using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.Extensions.Options;

namespace Web.Infrastructure
{
    public interface IAuthCookieFactory
    {
        CookieOptions AccessCookieOptions(DateTimeOffset expires);
        CookieOptions RefreshCookieOptions(DateTimeOffset expires);
        CookieOptions CsrfCookieOptions(DateTimeOffset expires);
    }

    public class AuthCookieFactory : IAuthCookieFactory
    {
        private readonly CookieSettings _settings;

        public AuthCookieFactory(IOptions<CookieSettings> cookieOptions)
        {
            _settings = cookieOptions.Value;
        }

        public CookieOptions AccessCookieOptions(DateTimeOffset expires) => new()
        {
            HttpOnly = true,
            Secure = _settings.secure || _settings.sameSiteMode == SameSiteMode.None, // por si config pide None
            SameSite = _settings.sameSiteMode,
            Expires = expires,                             // <-- DateTimeOffset correcto
            Path = _settings.path,
            Domain = _settings.domain,
            IsEssential = true
        };

        public CookieOptions RefreshCookieOptions(DateTimeOffset expires) => new()
        {
            HttpOnly = true,
            Secure = true,                                 // <-- obligatorio si SameSite=None
            SameSite = SameSiteMode.None,                  // si quieres forzarlo aquí
            Expires = expires,                             // <-- DateTimeOffset correcto
            Path = _settings.path,
            Domain = _settings.domain,
            IsEssential = true
        };

        public CookieOptions CsrfCookieOptions(DateTimeOffset expires) => new()
        {
            HttpOnly = false, // accesible por JS para double-submit
            Secure = true,    // <-- obligatorio si SameSite=None
            SameSite = SameSiteMode.None,                  // si quieres forzarlo aquí
            Expires = expires,                             // <-- DateTimeOffset correcto
            Path = _settings.path,
            Domain = _settings.domain,
            IsEssential = true
        };
    }

}

