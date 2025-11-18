using Data.Interfaces.DataBasic;
using Entity.Domain.Models.Implements.parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.IDataImplement.parameters
{
    public interface INotificationSettingRepository : IData<NotificationSetting>
    {

        public abstract Task<NotificationSetting?> FirstOrDefaultAsync(Expression<Func<NotificationSetting, bool>> predicate);
        Task<NotificationSetting?> GetLastUpdatedAsync();
    }
}
