using Data.Interfaces.IDataImplement.parameters;
using Data.Repositoy;
using Entity.Domain.Models.Implements.parameters;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Parameters
{
    public class NotificationSettingRepository : DataGeneric<NotificationSetting>, INotificationSettingRepository
    {
        public NotificationSettingRepository(ApplicationDbContext context) : base(context)
        {
        }


        public async Task<NotificationSetting?> FirstOrDefaultAsync(Expression<Func<NotificationSetting, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public override async Task<bool> UpdateAsync(NotificationSetting entity)
        {
            if (entity is NotificationSetting ns)
            {
                ns.UpdatedAt = DateTime.Now;
            }

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<NotificationSetting?> GetLastUpdatedAsync()
        {
            return await _dbSet
                .OrderByDescending(x => x.UpdatedAt)
                .FirstOrDefaultAsync();
        }

    }
}
