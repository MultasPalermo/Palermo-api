using Business.Interfaces.BusinessBasic;
using Entity.Domain.Models.Implements.parameters;
using Entity.DTOs.Default.parameters;
using Entity.DTOs.Select.parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces.IBusinessImplements.parameters
{
    public interface INotificationSettingServices : IBusiness<NotificationSettingDto, NotificationSettingSelect>
    {

        Task<int?> GetDaysByNameAsync(string name);
        // NUEVO
        Task<NotificationSettingDto?> GetLastUpdatedAsync();
    }
}
