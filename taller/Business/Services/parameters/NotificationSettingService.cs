    using AutoMapper;
    using Business.Interfaces.IBusinessImplements.parameters;
    using Business.Repository;
    using Data.Interfaces.DataBasic;
    using Data.Interfaces.IDataImplement.parameters;
    using Entity.Domain.Models.Implements.parameters;
    using Entity.DTOs.Default.parameters;
    using Entity.DTOs.Select.parameters;
    using Entity.Infrastructure.Contexts;
    using System.Threading.Tasks;

    namespace Business.Services.parameters
    {
        public class NotificationSettingService
          : BusinessBasic<NotificationSettingDto, NotificationSettingSelect, NotificationSetting>,
            INotificationSettingServices
        {
            private readonly INotificationSettingRepository _notificationRepo;
            private readonly IData<NotificationSetting> _data;

            public NotificationSettingService(
                INotificationSettingRepository notificationRepo,
                IData<NotificationSetting> data,
                IMapper mapper,
                ApplicationDbContext context
            ) : base(data, mapper, context)
            {
                _notificationRepo = notificationRepo;
                _data = data;
            }

            public async Task<int?> GetDaysByNameAsync(string name)
            {
                var setting = await _notificationRepo.FirstOrDefaultAsync(x => x.Name == name && x.active);
                return setting?.Days;
            }

            public async Task<NotificationSettingDto?> GetLastUpdatedAsync()
            {
                var entity = await _notificationRepo.GetLastUpdatedAsync();
                return _mapper.Map<NotificationSettingDto>(entity);
            }

        }
    }
