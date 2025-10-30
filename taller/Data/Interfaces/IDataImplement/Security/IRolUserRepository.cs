using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.DTOs.Default;

namespace Data.Interfaces.IDataImplement.Security
{
    public interface IRolUserRepository : IData<RolUser>
    {
        Task<IEnumerable<string>> GetJoinRolesAsync(int userId);
        Task<RolUser> AsignateUserRTo(User user);

        Task<List<string>> GetRoleNamesByUserIdAsync(int userId); // alias


    }
}
