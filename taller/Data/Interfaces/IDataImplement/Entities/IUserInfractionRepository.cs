using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.IDataImplement.Entities
{
    public interface IUserInfractionRepository : IData<UserInfraction>
    {
        Task<IEnumerable<UserInfraction>> GetByDocumentAsync(int documentTypeId, string documentNumber);
        Task<UserInfraction?> GetUserInfractionWithUserAndPersonAsync(int infractionId);
        Task<IEnumerable<UserInfraction>> GetByTypeInfractionAsync(int typeInfractionId);
        Task<IEnumerable<UserInfraction>> GetMultasAsync(int? documentTypeId, int? typeInfractionId, EstadoMulta? stateInfraction);

        }
}
