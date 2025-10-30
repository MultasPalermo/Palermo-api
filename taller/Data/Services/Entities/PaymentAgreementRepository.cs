using Data.Interfaces.IDataImplement.Entities;
using Data.Repositoy;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.parameters;
using Entity.Infrastructure.Contexts;
using Entity.Init;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PaymentAgreementRepository : DataGeneric<PaymentAgreement>, IPaymentAgreementRepository
{
    public PaymentAgreementRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<IEnumerable<PaymentAgreement>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.userInfraction)
                .ThenInclude(ui => ui.User)
                    .ThenInclude(u => u.Person)
            .Include(p => p.userInfraction.User.documentType)
            .Include(p => p.userInfraction.Infraction)
                .ThenInclude(i => i.TypeInfraction)
            .Include(p => p.userInfraction.Infraction)
                .ThenInclude(i => i.fineCalculationDetail)
                    .ThenInclude(fd => fd.valueSmldv)
            .Include(p => p.paymentFrequency)
            .Include(p => p.TypePayment)
            .Where(p => !p.is_deleted)
            .ToListAsync();
    }

    public override async Task<IEnumerable<PaymentAgreement>> GetDeletes()
    {
        return await _dbSet
            .Include(p => p.userInfraction)
                .ThenInclude(ui => ui.User)
                    .ThenInclude(u => u.Person)
            .Include(p => p.userInfraction.User.documentType)
            .Include(p => p.userInfraction.Infraction)
                .ThenInclude(i => i.TypeInfraction)
            .Include(p => p.userInfraction.Infraction)
                .ThenInclude(i => i.fineCalculationDetail)
                    .ThenInclude(fd => fd.valueSmldv)
            .Include(p => p.paymentFrequency)
            .Include(p => p.TypePayment)
            .Where(p => p.is_deleted)
            .ToListAsync();
    }

    public override async Task<PaymentAgreement?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.userInfraction)
                .ThenInclude(ui => ui.User)
                    .ThenInclude(u => u.Person)
            .Include(p => p.userInfraction.User.documentType)
            .Include(p => p.userInfraction.Infraction)
                .ThenInclude(i => i.TypeInfraction)
            .Include(p => p.userInfraction.Infraction)
                .ThenInclude(i => i.fineCalculationDetail)
                    .ThenInclude(fd => fd.valueSmldv)
            .Include(p => p.paymentFrequency)
            .Include(p => p.TypePayment)
            .Include(p => p.InstallmentSchedule) // ✅ Aquí agregas el cronograma
            .FirstOrDefaultAsync(p => p.id == id);
    }


    public async Task<IEnumerable<PaymentAgreementInitDto>> GetInitDataAsync(int userId, int? infractionId = null)
    {
        var infractionsQuery = _context.userInfraction
            .Where(ui => ui.UserId == userId) // ✅ filtramos por userId
            .Include(ui => ui.User)
                .ThenInclude(u => u.Person)
            .Include(ui => ui.User)
                .ThenInclude(u => u.documentType)
            .Include(ui => ui.Infraction)
                .ThenInclude(i => i.TypeInfraction)
            .Include(ui => ui.Infraction)
                .ThenInclude(i => i.fineCalculationDetail)
                    .ThenInclude(fd => fd.valueSmldv);


        var infractions = await infractionsQuery.ToListAsync();

        return infractions.Select(infraction =>
        {
            var detail = infraction.Infraction?.fineCalculationDetail?
                .OrderByDescending(fd => fd.valueSmldv.Current_Year)
                .FirstOrDefault();

            return new PaymentAgreementInitDto
            {
                PersonName = $"{infraction.User.Person?.firstName ?? ""} {infraction.User.Person?.lastName ?? ""}".Trim(),
                DocumentNumber = infraction.User?.documentNumber ?? string.Empty,
                DocumentType = infraction.User?.documentType?.name ?? string.Empty,
                InfractionId = infraction.id,
                Infringement = infraction.Infraction?.TypeInfraction?.Name ?? string.Empty,
                TypeFine = infraction.Infraction?.description ?? string.Empty,
                ValorSMDLV = (decimal)(detail?.valueSmldv?.value_smldv ?? 0),
                BaseAmount = detail != null
                    ? (detail.totalCalculation > 0
                        ? detail.totalCalculation
                        : (detail.Infraction?.numer_smldv ?? 0) * (decimal)(detail.valueSmldv?.value_smldv ?? 0))
                    : 0,
                UserId = infraction.UserId
            };
        });
    }

    public async Task<UserInfraction?> GetUserInfractionWithDetailsAsync(int userInfractionId)
    {
        return await _context.userInfraction
            .Include(ui => ui.User)
                .ThenInclude(u => u.Person)
            .Include(ui => ui.Infraction)
                .ThenInclude(i => i.TypeInfraction)
            .Include(ui => ui.Infraction)
                .ThenInclude(i => i.fineCalculationDetail)
                    .ThenInclude(fd => fd.valueSmldv)
            .FirstOrDefaultAsync(ui => ui.id == userInfractionId);
    }

    public async Task<PaymentFrequency?> GetPaymentFrequencyAsync(int id)
        => await _context.paymentFrequency.FindAsync(id);

    public async Task<TypePayment?> GetTypePaymentAsync(int id)
        => await _context.typePayment.FindAsync(id);
}
