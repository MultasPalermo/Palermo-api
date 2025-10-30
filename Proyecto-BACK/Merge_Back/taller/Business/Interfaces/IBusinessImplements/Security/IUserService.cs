using Business.Interfaces.BusinessBasic;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.LoginDto.response.RegisterReponseDto;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Default.RegisterRequestDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;

namespace Business.Interfaces.IBusinessImplements.Security
{
    public interface IUserService : IBusiness<UserDto, UserSelectDto>
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<User> createUserGoogle(string email, string name);
        Task<bool> VerifyCodeAsync(string code);


    }
}
