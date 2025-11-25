using Data.Interfaces.IDataImplement.parameters;
using Data.Repositoy;
using Entity.Domain.Models.Implements.parameters;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Parameters
{
    public class PaymentFrequencyRepository : DataGeneric<PaymentFrequency>, IPaymentFrequencyRepository
    {
        public PaymentFrequencyRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<PaymentFrequency?> GetByIntervalAsync(string interval)
        {
            return await _context.Set<PaymentFrequency>()
                .FirstOrDefaultAsync(f => f.intervalPage.ToUpper() == interval.ToUpper());
        }
    }
}
