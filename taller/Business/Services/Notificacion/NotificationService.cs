using AutoMapper;
using Business.Interfaces.Notificacion;
using Business.Repository;
using Data.Interfaces.IDataImplement.Notificacion;
using Entity.Domain.Models.Implements.Notificacion;
using Entity.DTOs.Default.Notificacion;
using Helpers.Initialize;
using Utilities.Exceptions;
using static Entity.Domain.Enums.Notification.NotificationEnums;

namespace Business.Services.Notificacion
{
    public class NotificationService : BusinessBasic<NotificationCreateDto, NotificationDto,  Notification>,
          INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly INotificationRealtimeService _realtime;

        public NotificationService(
            INotificationRepository repository,
            INotificationRealtimeService realtime,
            IMapper mapper)
            : base(repository, mapper)
        {
            _repository = repository;
            _realtime = realtime;
        }

        public override async Task<NotificationCreateDto> CreateAsync(NotificationCreateDto dto)
        {
            var notification = _mapper.Map<Notification>(dto);
            notification.Status = NotificationStatus.Unread;

            var created = await _repository.CreateAsync(notification);

            // DTO que usa el realtime (tipo que espera PushAsync)
            var pushDto = _mapper.Map<NotificationDto>(created);
            await _realtime.PushAsync(pushDto);

            // DTO que devuelve el método (mantener tu firma actual)
            var dtoCreated = _mapper.Map<NotificationCreateDto>(created);
            return dtoCreated;
        }

        public override async Task<bool> UpdateAsync(NotificationCreateDto dto)
        {
            if (dto == null)
                throw new BusinessException("El DTO no puede ser nulo.");

            var entity = await _repository.GetByIdAsync(dto.id)
                ?? throw new BusinessException("La notificación no existe.");

            _mapper.Map(dto, entity);
            entity.InitializeLogicalState(); // si aplica en tu dominio

            // Asumimos que UpdateAsync devuelve bool
            var updated = await _repository.UpdateAsync(entity);
            return updated;
        }


        public async Task<IReadOnlyList<NotificationDto>> GetFeedAsync(int userId, NotificationStatus? status = null, int take = 20)
        {
            var notifications = await _repository.GetByUserAsync(userId, status, take);
            return notifications.Select(n => _mapper.Map<NotificationDto>(n)).ToList();
        }

        public async Task<IReadOnlyList<NotificationDto>> GetUnreadAsync(int userId)
        {
            var notifications = await _repository.GetUnreadByUserAsync(userId);
            return notifications.Select(n => _mapper.Map<NotificationDto>(n)).ToList();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _repository.GetByIdAsync(notificationId);
            if (notification is null || notification.RecipientUserId != userId || notification.is_deleted)
            {
                return false;
            }

            if (notification.Status == NotificationStatus.Read)
            {
                return true;
            }

            notification.Status = NotificationStatus.Read;
            notification.ReadAt = DateTime.UtcNow;
            await _repository.UpdateAsync(notification);
            return true;
        }

        public Task<int> MarkAllAsReadAsync(int userId)
        {
            return _repository.MarkAllAsReadAsync(userId);
        }
    }
}

