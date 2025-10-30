
using Entity.DTOs.Default.Auth;
using Entity.DTOs.Default.LoginDto;
using Google.Apis.Auth;
namespace Business.Interfaces.IJWT
{
    public interface IToken
    {
        /// <summary>
        /// Valida credenciales y emite Access + Refresh + CSRF.
        /// </summary>
        Task<(string AccessToken, string RefreshToken, string CsrfToken)> GenerateTokensAsync(LoginDto dto);
        Task<(string NewAccessToken, string NewRefreshToken)> RefreshAsync(string refreshTokenPlain, string? remoteIp = null);
        Task RevokeRefreshTokenAsync(string refreshToken);
        //Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string tokenId);
        //Task<string> GenerateTokenEmail(EmailLoginDto dto);
        //bool validarToken(string token);
    }
}
