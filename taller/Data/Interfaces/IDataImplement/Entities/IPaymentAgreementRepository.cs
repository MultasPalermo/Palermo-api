using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.parameters;
using Entity.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.IDataImplement.Entities
{
    public interface IPaymentAgreementRepository : IData<PaymentAgreement>
    {
        Task<IEnumerable<PaymentAgreementInitDto>> GetInitDataAsync(int userId, int? infractionId = null);
        Task<UserInfraction?> GetUserInfractionWithDetailsAsync(int userInfractionId);
        Task<PaymentFrequency?> GetPaymentFrequencyAsync(int id);
        Task<TypePayment?> GetTypePaymentAsync(int id);
    }
}
