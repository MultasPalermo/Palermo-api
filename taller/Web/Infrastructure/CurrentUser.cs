using Business.CustomJWT;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.Infrastructure
{
    /// <summary>
    /// Implementación concreta de <see cref="ICurrentUser"/> que obtiene
    /// la información del usuario autenticado a partir del contexto HTTP actual.
    ///
    /// Esta clase se integra con el middleware de autenticación de ASP.NET Core
    /// (por ejemplo, JWT Bearer) y extrae los datos de los claims contenidos
    /// en el token del usuario.
    ///
    /// Se declara como <c>sealed</c> para evitar herencia y mantener la
    /// inmutabilidad del comportamiento.
    /// </summary>
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _ctx;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CurrentUser"/>.
        /// </summary>
        /// <param name="ctx">
        /// Accesor al contexto HTTP (<see cref="IHttpContextAccessor"/>),
        /// utilizado para acceder al usuario autenticado y sus claims.
        /// </param>
        public CurrentUser(IHttpContextAccessor ctx) => _ctx = ctx;

        /// <summary>
        /// Obtiene el identificador de persona (<c>person_id</c>) del usuario actual,
        /// si existe en los claims del token JWT.
        /// </summary>
        /// <remarks>
        /// Si el claim <c>person_id</c> no está presente o no puede convertirse
        /// a un número entero, devuelve <c>null</c>.
        /// </remarks>
        public int? PersonId =>
            int.TryParse(_ctx.HttpContext?.User?.FindFirst("person_id")?.Value, out var id) ? id : null;

        /// <summary>
        /// Obtiene el identificador interno del usuario autenticado.
        /// </summary>
        /// <remarks>
        /// Se extrae usando el claim estándar <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>,
        /// el cual es el mecanismo recomendado por ASP.NET Core, OpenID Connect y ASP.NET Identity
        /// para representar el "user id" dentro del token.
        ///
        /// Si el claim no está presente o no se puede convertir a entero,
        /// el valor devuelto será <c>null</c>.
        ///
        /// Esta elección permite:
        /// - evitar nombres de claim "custom" no interoperables
        /// - mantener compatibilidad futura con IdentityServer/OpenIddict/Entra
        /// - no depender de literales como "user_id"
        ///
        /// Fuentes oficiales:
        /// - https://learn.microsoft.com/dotnet/api/system.security.claims.claimtypes.nameidentifier
        /// </remarks>
        public int? UserId =>
            int.TryParse(
                _ctx.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? _ctx.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                out var id) ? id : null;


    }
}