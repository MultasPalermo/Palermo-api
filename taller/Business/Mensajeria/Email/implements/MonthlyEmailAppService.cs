// Business.Mensajeria.Email.implements/MonthlyEmailAppService.cs
using Business.Mensajeria.Email.@interface;
using Data.Interfaces.IDataImplement.Security; // IUserRepository
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business.Mensajeria.Email.implements
{
    public class MonthlyEmailAppService
{
    private readonly IServiceEmail _email;
    private readonly IUserRepository _users;
    private readonly ApplicationDbContext _db; // tu AppDbContext
    private readonly ILogger<MonthlyEmailAppService> _logger;

    public MonthlyEmailAppService(
        IServiceEmail email,
        IUserRepository users,
        ApplicationDbContext db, // inyecta AppDbContext
        ILogger<MonthlyEmailAppService> logger)
    {
        _email = email;
        _users = users;
        _db = db;
        _logger = logger;
    }

    // Se ejecuta el día 4 por el worker
    public async Task EjecutarEnvioMensualAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var pendientes = await _users.GetUnverifiedUsersAsync(ct);
        if (pendientes.Count == 0)
        {
            _logger.LogInformation("No hay usuarios pendientes de verificación.");
            return;
        }

        // 1) Generar/renovar códigos (si no hay uno vigente)
        int generados = 0;
        foreach (var u in pendientes)
        {
            // si ya tiene un código Y NO ha vencido, no re-enviamos (antispam)
            if (u.EmailVerificationExpiresAt.HasValue && u.EmailVerificationExpiresAt.Value > now
                && !string.IsNullOrWhiteSpace(u.EmailVerificationCode))
            {
                continue;
            }

            // generar nuevo
            var code = new Random().Next(100000, 999999).ToString();
            u.EmailVerificationCode = code;
            u.EmailVerificationExpiresAt = now.AddHours(24);
            generados++;
        }

        if (generados > 0)
            await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Usuarios pendientes: {Total}. Codigos generados/renovados: {Gen}.",
                               pendientes.Count, generados);

        // 2) Enviar correos (a todos los pendientes, si tienen código vigente)
        int enviados = 0, saltados = 0;
        foreach (var u in pendientes)
        {
            if (string.IsNullOrWhiteSpace(u.email)) { saltados++; continue; }
            if (string.IsNullOrWhiteSpace(u.EmailVerificationCode) ||
                !(u.EmailVerificationExpiresAt.HasValue && u.EmailVerificationExpiresAt.Value > now))
            {   // sin código vigente → nada que enviar
                saltados++; continue;
            }

            try
            {
                    var builder = new VerificacionEmailBuilder(
    u.Person?.firstName ?? "Usuario",
    u.EmailVerificationCode
);

                    await _email.SendEmailAsyncVerificacion(u.email!, builder);

                }
                catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando código a {Email}", u.email);
            }
        }

        _logger.LogInformation("Envio mensual verificación: enviados={Enviados}, saltados={Saltados}.", enviados, saltados);
    }
}
}
