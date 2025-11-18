using Data.Interfaces.IDataImplement.Notificacion;
using Data.Repositoy;
using Entity.Domain.Models.Implements.Notificacion;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using static Entity.Domain.Enums.Notification.NotificationEnums;

namespace Data.Services.Notificacion
{
    public class NotificationRepository : DataGeneric<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .Where(n => !n.is_deleted)
                .OrderByDescending(n => n.created_date)
                .ThenByDescending(n => n.id)
                .ToListAsync();
        }

        public override async Task<Notification?> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(n => n.id == id && !n.is_deleted);
        }

        public async Task<IReadOnlyList<Notification>> GetUnreadByUserAsync(int userId)
        {
            return await _dbSet.AsNoTracking()
                .Where(n => n.RecipientUserId == userId && !n.is_deleted && n.Status == NotificationStatus.Unread)
                .OrderByDescending(n => n.created_date)
                .ThenByDescending(n => n.id)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Notification>> GetByUserAsync(int userId, NotificationStatus? status = null, int take = 20)
        {
            var query = _dbSet.AsNoTracking()
                .Where(n => n.RecipientUserId == userId && !n.is_deleted);

            if (status.HasValue)
            {
                query = query.Where(n => n.Status == status.Value);
            }

            query = query
                .OrderByDescending(n => n.created_date)
                .ThenByDescending(n => n.id);

            if (take > 0)
            {
                query = query.Take(take);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> HasRecentNotificationAsync(int recipientUserId, NotificationType type, string? actionRoute, DateTime since)
        {
            var query = _dbSet.AsNoTracking()
                .Where(n => n.RecipientUserId == recipientUserId
                            && !n.is_deleted
                            && n.Type == type
                            && n.created_date >= since);

            query = actionRoute is null
                ? query.Where(n => n.ActionRoute == null)
                : query.Where(n => n.ActionRoute == actionRoute);

            return await query.AnyAsync();
        }

        public async Task<int> MarkAllAsReadAsync(int userId)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .Where(n => n.RecipientUserId == userId && !n.is_deleted && n.Status == NotificationStatus.Unread)
                .ExecuteUpdateAsync(updates => updates
                    .SetProperty(n => n.Status, _ => NotificationStatus.Read)
                    .SetProperty(n => n.ReadAt, _ => now));
        }
    }
}

