using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.Entities;

namespace Data.Interfaces.IDataImplement.Entities
{
    public interface IUserInfractionRepository : IData<UserInfraction>
    {
        Task<IEnumerable<UserInfraction>> GetByDocumentAsync(int documentTypeId, string documentNumber);
        Task<UserInfraction?> GetUserInfractionWithUserAndPersonAsync(int infractionId);
        Task<IEnumerable<UserInfraction>> GetByTypeInfractionAsync(int typeInfractionId);

    }
}
