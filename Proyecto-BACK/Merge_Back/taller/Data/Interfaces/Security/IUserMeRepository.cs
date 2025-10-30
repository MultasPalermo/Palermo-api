using Entity.Domain.Models.Implements.ModelSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.Security
{
    public interface IUserMeRepository
    {
        Task<User?> GetUserWithPersonAsync(int userId);
        Task<IEnumerable<RolUser>> GetUserRolesWithPermissionsAsync(int userId);
        Task<IEnumerable<Form>> GetFormsWithModulesByIdsAsync(List<int> formIds);
        Task<User?> GetUserWithFullContextAsync(int userId);

    }
}
