using Business.Interfaces.BusinessBasic;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default.ModelSecurityDto;
using Entity.DTOs.Select.ModelSecuritySelectDto;

namespace Business.Interfaces.IBusinessImplements.Security
{
    public interface IRolUserService : IBusiness<RolUserDto, RolUserSelectDto>
    {
        //Task<IEnumerable<RolUserDto>> GetAllRolUsersAsync();
        //Task AddRolUserAsync(RolUserDto dto);
        //Task DeleteRolUserAsync(int rolId, int userId);
        Task<IEnumerable<string>> GetAllRolUser(int idUser);
        Task<RolUserDto> AsignateUserRTo(User user);
    }
}
