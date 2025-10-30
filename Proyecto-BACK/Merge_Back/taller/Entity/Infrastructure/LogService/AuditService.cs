//using Entity.Domain.Interfaces;
//using Entity.Domain.Models;
//using Entity.Infrastructure.Contexts;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using System.Text.Json;
//using System.Transactions;

//namespace Entity.Infrastructure.LogService
//{
//    public class AuditService : IAuditService
//    {
//        private readonly AuditDbContext _auditCtx;
//        private readonly IHttpContextAccessor _http;
//        private readonly JsonSerializerOptions _jsonOptions;

//        public AuditService(AuditDbContext auditCtx, IHttpContextAccessor http)
//        {
//            _auditCtx = auditCtx;
//            _http = http;
//            _jsonOptions = new JsonSerializerOptions
//            {
//                WriteIndented = false,
//                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
//            };
//        }

//        public async Task CaptureAsync(ChangeTracker tracker, CancellationToken ct = default)
//        {
//            try
//            {
//                var userId = _http.HttpContext?.User?.FindFirst("id")?.Value ?? "anonymous";
//                var now = DateTime.UtcNow;

//                // Optimización: Evaluación temprana de cambios para evitar procesamiento innecesario
//                var changedEntries = tracker.Entries()
//                    .Where(e => e.State is EntityState.Added
//                             or EntityState.Modified
//                             or EntityState.Deleted)
//                    .ToList();

//                if (changedEntries.Count == 0)
//                    return;

//                var logs = new List<audit>(changedEntries.Count);

//                foreach (var entry in changedEntries)
//                {
//                    var keys = GetPrimaryKeys(entry);
//                    var (oldValues, newValues) = GetChangedValues(entry);

//                    logs.Add(new audit
//                    {
//                        Entity = entry.Metadata.GetTableName() ?? entry.Metadata.ClrType.Name,
//                        action = entry.State.ToString(),
//                        keyValues = JsonSerializer.Serialize(keys, _jsonOptions),
//                        oldValues = oldValues.Count > 0 ? JsonSerializer.Serialize(oldValues, _jsonOptions) : null,
//                        newValues = newValues.Count > 0 ? JsonSerializer.Serialize(newValues, _jsonOptions) : null,
//                        userId = userId,
//                        dateTime = now
//                    });
//                }

//                // Optimización: Usar transacciones para operaciones que afecten a muchas entidades
//                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
//                {
//                    // Guardar en lotes para mejor rendimiento
//                    await _auditCtx.audit.AddRangeAsync(logs, ct);
//                    await _auditCtx.SaveChangesAsync(ct);

//                    transaction.Complete();
//                }
//            }
//            catch (Exception ex)
//            {
//                // Registrar el error pero permitir que la operación principal continúe
//                // Opcionalmente, implementar un mecanismo de registro de errores aquí
//                System.Diagnostics.Debug.WriteLine($"Error en auditoría: {ex.Message}");
//            }
//        }

//        private static Dictionary<string, object?> GetPrimaryKeys(EntityEntry entry)
//        {
//            return entry.Properties
//                .Where(p => p.Metadata.IsPrimaryKey())
//                .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
//        }

//        private static (Dictionary<string, object?> OldValues, Dictionary<string, object?> NewValues) GetChangedValues(EntityEntry entry)
//        {
//            var oldValues = new Dictionary<string, object?>(StringComparer.Ordinal);
//            var newValues = new Dictionary<string, object?>(StringComparer.Ordinal);

//            switch (entry.State)
//            {
//                case EntityState.Added:
//                    foreach (var prop in entry.Properties)
//                    {
//                        if (prop.CurrentValue != null)
//                            newValues[prop.Metadata.Name] = prop.CurrentValue;
//                    }
//                    break;

//                case EntityState.Deleted:
//                    foreach (var prop in entry.Properties)
//                    {
//                        if (prop.OriginalValue != null)
//                            oldValues[prop.Metadata.Name] = prop.OriginalValue;
//                    }
//                    break;

//                case EntityState.Modified:
//                    var databaseValues = entry.GetDatabaseValues();
//                    if (databaseValues != null)
//                    {
//                        foreach (var prop in entry.Properties)
//                        {
//                            var originalValue = databaseValues[prop.Metadata.Name];
//                            var currentValue = prop.CurrentValue;

//                            // Optimización: Solo registrar los cambios reales
//                            if (!Equals(originalValue, currentValue))
//                            {
//                                oldValues[prop.Metadata.Name] = originalValue;
//                                newValues[prop.Metadata.Name] = currentValue;
//                            }
//                        }
//                    }
//                    break;
//            }

//            return (oldValues, newValues);
//        }
//    }
//}