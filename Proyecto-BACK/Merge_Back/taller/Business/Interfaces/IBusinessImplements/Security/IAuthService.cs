using Entity.DTOs.Default.Auth;
using Entity.DTOs.Default.Auth.RestPasword;
using Entity.DTOs.Default.Me;
using Entity.DTOs.Default.ModelSecurityDto;

namespace Business.Interfaces.IBusinessImplements.Security
{
   public  interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterDto dto);
        Task RequestPasswordResetAsync(string email);
        Task ResetPasswordAsync(ConfirmResetDto dto);
        Task<UserMeDto> BuildUserContextAsync(int userId);
    }
}
