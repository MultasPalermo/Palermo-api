using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Entity.Domain.Interfaces
{
    public interface IAuditService
    {
        Task CaptureAsync(ChangeTracker tracker, CancellationToken ct = default);
    }

}
