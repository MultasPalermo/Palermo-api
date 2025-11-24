using AutoMapper;
using Business.Interfaces.IBusinessImplements.parameters;
using Business.Interfaces.PDF;
using Business.Mensajeria.Email.@interface;
using Business.Mensajeria.Email.SignalR;
using Data.Interfaces.IDataImplement.Entities;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    /// <summary>
    /// servicio que programa y envía recordatorios automáticos (3, 15 y 25 días)
    /// con sus respectivos pdfs y plantillas diferentes.
    /// </summary>
    public class ReminderEmailAppService
    {
        private readonly IServiceEmail _emailService;
        private readonly IPdfGeneratorService _pdfService;
        private readonly EmailScheduler _scheduler;
        private readonly ILogger<ReminderEmailAppService> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<MultasHub> _hub;
        private readonly IInfractionDiscountRunner _discountRunner;

        public ReminderEmailAppService(
            IServiceEmail emailService,
            IPdfGeneratorService pdfService,
            EmailScheduler scheduler,
            ILogger<ReminderEmailAppService> logger,
            IMapper mapper,
            IServiceScopeFactory scopeFactory,
            IHubContext<MultasHub> hub,
            IInfractionDiscountRunner discountRunner)
        {
            _emailService = emailService;
            _pdfService = pdfService;
            _scheduler = scheduler;
            _logger = logger;
            _mapper = mapper;
            _scopeFactory = scopeFactory;
            _hub = hub;
            _discountRunner = discountRunner;
        }

        // --- ProgramarRecordatoriosAsync ---
        private static readonly ConcurrentDictionary<int, bool> _multasProgramadas = new();

        // --- ProgramarRecordatoriosAsync ---
        public async Task ProgramarRecordatoriosAsync(UserInfractionSelectDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("❌ dto nulo. no se pueden programar recordatorios.");
                return;
            }

            // 🔒 PROTECCIÓN: Verificar si ya se programaron recordatorios para esta multa
            if (!_multasProgramadas.TryAdd(dto.id, true))
            {
                _logger.LogWarning($"🛑 Los recordatorios para la multa #{dto.id} YA FUERON PROGRAMADOS. Ignorando llamada duplicada.");
                return;
            }

            // Clonación del record para inmutabilidad.
            var dtoSeguro = dto with { };

            if (dtoSeguro.stateInfraction == EstadoMulta.ConAcuerdoPago)
            {
                _logger.LogInformation($"🛑 La infracción #{dtoSeguro.id} ya tiene estado 'ConAcuerdoPago'. No se programarán recordatorios.");
                _multasProgramadas.TryRemove(dtoSeguro.id, out _); // Limpiar del diccionario
                return;
            }

            if (string.IsNullOrWhiteSpace(dtoSeguro.userEmail))
            {
                _logger.LogWarning($"⚠️ La infracción #{dtoSeguro.id} no tiene correo asignado.");
                _multasProgramadas.TryRemove(dtoSeguro.id, out _);
                return;
            }

            if (dtoSeguro.dateInfraction == default)
            {
                _logger.LogWarning($"⚠️ La infracción #{dtoSeguro.id} no tiene fecha válida.");
                _multasProgramadas.TryRemove(dtoSeguro.id, out _);
                return;
            }

            _logger.LogInformation($"📅 [PRIMERA VEZ] Programando recordatorios para infracción #{dtoSeguro.id} ({dtoSeguro.userEmail})...");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var settingService = scope.ServiceProvider.GetRequiredService<INotificationSettingServices>();
                var repoScoped = scope.ServiceProvider.GetRequiredService<IUserInfractionRepository>();

                var currentEntity = await repoScoped.GetByIdAsync(dtoSeguro.id);
                if (currentEntity == null)
                {
                    _logger.LogWarning($"⚠ La infracción #{dtoSeguro.id} no existe en BD. No se programarán recordatorios.");
                    _multasProgramadas.TryRemove(dtoSeguro.id, out _);
                    return;
                }
                var currentStatus = currentEntity.StatusCollection;

                var settings = (await settingService.GetAllAsync())?.ToList();

                if (settings == null || settings.Count < 5)
                {
                    _logger.LogWarning("⚠️ No están configurados los recordatorios 1 a 5.");
                    _multasProgramadas.TryRemove(dtoSeguro.id, out _);
                    return;
                }

                var r1 = settings.FirstOrDefault(x => x.id == 1);
                var r2 = settings.FirstOrDefault(x => x.id == 2);
                var r3 = settings.FirstOrDefault(x => x.id == 3);
                var r4 = settings.FirstOrDefault(x => x.id == 4);
                var r5 = settings.FirstOrDefault(x => x.id == 5);

                if (r1 == null || r2 == null || r3 == null || r4 == null || r5 == null)
                {
                    _logger.LogWarning("⚠️ Faltan recordatorios (IDs 1 a 5).");
                    _multasProgramadas.TryRemove(dtoSeguro.id, out _);
                    return;
                }

                string timeUnit = r1.TimeUnit?.ToUpper() ?? "DAYS";
                bool usarSegundos = timeUnit == "SECONDS";

                _logger.LogWarning($"⏱ Modo utilizado: {(usarSegundos ? "SEGUNDOS (pruebas)" : "DÍAS (normal)")}");

                DateTime CalcFecha(int val) => usarSegundos ? dtoSeguro.dateInfraction.AddSeconds(val) : dtoSeguro.dateInfraction.AddDays(val);
                TimeSpan CalcDelay(DateTime target) => target - DateTime.Now;
                string Etiqueta(int val) => usarSegundos ? $"{val} segundos" : $"{val} días";

                var fechas = new[]
                {
            (EstadoCobro.prejuridico3Dias, r1.Days, CalcFecha(r1.Days), Etiqueta(r1.Days)),
            (EstadoCobro.prejuridico15Dias, r2.Days, CalcFecha(r2.Days), Etiqueta(r2.Days)),
            (EstadoCobro.prejuridico25Dias, r3.Days, CalcFecha(r3.Days), Etiqueta(r3.Days)),
            (EstadoCobro.CobroJuridico, r4.Days, CalcFecha(r4.Days), Etiqueta(r4.Days)),
            (EstadoCobro.CobroCoactivo, r5.Days, CalcFecha(r5.Days), Etiqueta(r5.Days))
        };

                foreach (var (targetStatus, valor, fechaEnvio, etiqueta) in fechas)
                {
                    if (currentStatus >= targetStatus)
                    {
                        _logger.LogWarning($"❌ El estado actual ({currentStatus}) ya alcanzó o superó el recordatorio para {targetStatus}. No se programa.");
                        continue;
                    }

                    var delay = usarSegundos ? TimeSpan.FromSeconds(valor) : CalcDelay(fechaEnvio);

                    if (delay <= TimeSpan.Zero)
                    {
                        _logger.LogWarning($"⚠️ Ya pasó la fecha del recordatorio ({etiqueta}) para #{dtoSeguro.id}. No se programará.");
                        continue;
                    }

                    // ❌ ELIMINAR ESTAS LÍNEAS:
                    // var microsecondsOffset = (dtoSeguro.id % 100) * 10;
                    // delay = delay.Add(TimeSpan.FromMilliseconds(microsecondsOffset));

                    // ✅ AGREGAR SOLO ESTO (opcional, para debuggear):
                    _logger.LogDebug($"🕐 Job para multa #{dtoSeguro.id}, estado {targetStatus}, delay: {delay.TotalSeconds:F3}s");

                    string jobId = $"Infraction_{dtoSeguro.id}_Status_{targetStatus}";
                    var dtoJob = dtoSeguro with { };

                    await ProgramarEnvioAsync(dtoJob, valor, etiqueta, delay, targetStatus, jobId);
                    _logger.LogInformation($"⏳ Recordatorio programado: {etiqueta} → En {delay.TotalSeconds:F3}s (Para el estado {targetStatus})");
                }

                _logger.LogInformation($"✅ Todos los recordatorios programados exitosamente para multa #{dtoSeguro.id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al programar recordatorios para infracción #{dtoSeguro.id}");
                // En caso de error, remover del diccionario para permitir reintento
                _multasProgramadas.TryRemove(dtoSeguro.id, out _);
            }
        }

        private int ObtenerRecordatorioIdPorEstado(EstadoCobro estado)
        {
            return estado switch
            {
                EstadoCobro.prejuridico3Dias => 1,
                EstadoCobro.prejuridico15Dias => 2,
                EstadoCobro.prejuridico25Dias => 3,
                EstadoCobro.CobroJuridico => 4,
                EstadoCobro.CobroCoactivo => 5,
                _ => 0
            };
        }

        private async Task ProgramarEnvioAsync(UserInfractionSelectDto dto, int dias, string etiqueta, TimeSpan delay, EstadoCobro targetStatus, string jobId)
        {
            var dtoCapturado = dto with { };
            var etiquetaCapturada = etiqueta;

            await _scheduler.ScheduleEmailAsync(async () =>
            {
                var dtoInmutable = dtoCapturado;
                var etiquetaParaLog = etiquetaCapturada;

                try
                {
                    // Usar etiquetaParaLog si quieres el log más preciso al momento de la ejecución.
                    _logger.LogInformation($"🚀 Enviando recordatorio de {etiquetaParaLog} a {dtoInmutable.userEmail}...");

                    using var scope = _scopeFactory.CreateScope();
                    var repoScoped = scope.ServiceProvider.GetRequiredService<IUserInfractionRepository>();
                    var infractionRunner = scope.ServiceProvider.GetRequiredService<IInfractionDiscountRunner>();
                    var hub = scope.ServiceProvider.GetRequiredService<IHubContext<MultasHub>>();

                    var entity = await repoScoped.GetByIdAsync(dtoInmutable.id);
                    if (entity == null)
                    {
                        _logger.LogWarning($"⚠ La infracción #{dtoInmutable.id} no existe en BD.");
                        return;
                    }

                    if (entity.stateInfraction == EstadoMulta.ConAcuerdoPago)
                    {
                        _logger.LogInformation($"🛑 Multa #{dtoInmutable.id} tiene estado 'ConAcuerdoPago'. Se aborta el envío del recordatorio para {targetStatus}.");
                        return;
                    }

                    if (entity.StatusCollection >= targetStatus)
                    {
                        _logger.LogInformation($"🛑 Multa #{dtoInmutable.id} ya en estado {entity.StatusCollection}. No se necesita enviar el recordatorio para {targetStatus}.");
                        return;
                    }

                    var oldStatus = entity.StatusCollection;
                    entity.StatusCollection = targetStatus;

                    // Actualizamos la copia local (dtoInmutable) para el cuerpo del correo/PDF.
                    dtoInmutable = dtoInmutable with { StatusCollection = entity.StatusCollection.ToString() };
                    var estado = entity.StatusCollection;

                    _logger.LogInformation($"📌 Estado actualizado ␦ {estado}");

                    if (oldStatus == EstadoCobro.CobroPrejuridico && estado == EstadoCobro.prejuridico3Dias)
                    {
                        _logger.LogWarning($"💰 Estado cambiado a {EstadoCobro.prejuridico3Dias}. Forzando recálculo (debería quitar el 50%).");

                        await repoScoped.UpdateAsync(entity);
                        await infractionRunner.RunOnceFor(entity.id);
                        _logger.LogInformation($"✅ Recálculo de descuento forzado para multa #{entity.id} vía IInfractionDiscountRunner.");

                        var updatedEntity = await repoScoped.GetByIdAsync(entity.id);
                        if (updatedEntity != null)
                        {
                            dtoInmutable = dtoInmutable with { amountToPay = updatedEntity.amountToPay };
                            _logger.LogInformation($"📝 Monto actualizado después del recálculo: {dtoInmutable.amountToPay:N0}");
                        }
                    }
                    else
                    {
                        await repoScoped.UpdateAsync(entity);
                    }

                    var (subject, body) = ObtenerContenidoCorreo(dtoInmutable, estado);
                    int reminderId = ObtenerRecordatorioIdPorEstado(estado);
                    byte[]? pdfBytes = null;

                    if (reminderId > 0)
                    {
                        pdfBytes = await _pdfService.GenerateReminderPdfAsync(dtoInmutable, reminderId);
                    }

                    if (pdfBytes != null)
                    {
                        _logger.LogInformation($"📎 Adjuntando PDF para recordatorio {reminderId}");

                        using var attachment = new Attachment(new MemoryStream(pdfBytes),
                                                             $"Recordatorio_{dtoInmutable.id}_R{reminderId}.pdf");

                        await _emailService.SendEmailAsync(
                            dtoInmutable.userEmail, subject, body, new List<Attachment> { attachment }
                        );
                    }
                    else
                    {
                        _logger.LogInformation($"📨 Envío sin PDF (recordatorio {reminderId})");
                        await _emailService.SendEmailAsync(dtoInmutable.userEmail, subject, body);
                    }

                    _logger.LogInformation($"✅ Recordatorio enviado correctamente a {dtoInmutable.userEmail}");
                    await hub.Clients.All.SendAsync("ReceiveStatusUpdate", dtoInmutable.id, dtoInmutable.StatusCollection);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"❌ Error al enviar recordatorio de {etiquetaParaLog} a {dtoInmutable.userEmail}");
                }
            }, delay, jobId);
        }


        private (string subject, string body) ObtenerContenidoCorreo(UserInfractionSelectDto dto, EstadoCobro estado)
        {
            string subject;
            string body;

            switch (estado)
            {

                case EstadoCobro.prejuridico3Dias:
                    subject = $"primer recordatorio - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p>" +
                           "<p>han pasado 3 días desde la notificación. aún tiene un pago pendiente.</p>";
                    break;

                case EstadoCobro.prejuridico15Dias:
                    subject = $"Segundo aviso - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p>" +
                           "<p>han pasado 15 días sin pago. si no cancela pronto, se iniciará cobro coactivo.</p>";
                    break;

                case EstadoCobro.prejuridico25Dias:
                    subject = $"Tercer recordatorio - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p>" +
                           "<p>han pasado 25 días desde la infracción. se le recuerda el pago pendiente.</p>";
                    break;

                case EstadoCobro.CobroJuridico:
                    subject = $"cobro jurídico - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p>" +
                           "<p>su proceso ha sido escalado a cobro jurídico.</p>";
                    break;

                case EstadoCobro.CobroCoactivo:
                    subject = $"cobro coactivo - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p>" +
                           "<p>su proceso se encuentra en cobro coactivo.</p>";
                    break;

                default:
                    subject = $"recordatorio de pago - infracción #{dto.id}";
                    body = $"<p>estimado/a {dto.firstName} {dto.lastName},</p><p>tiene un pago pendiente.</p>";
                    break;
            }

            body +=
                $"<p>fecha de la infracción: <b>{dto.dateInfraction:dd 'de' MMMM 'de' yyyy}</b></p>" +
                $"<p>monto a pagar: <b>${dto.amountToPay:N0}</b></p>" +
                "<p>atentamente,<br/>secretaría de tránsito municipal</p>";

            return (subject, body);
        }


        public async Task ActualizarEstadoMultaAsync(UserInfraction entity, EstadoCobro nuevoEstado)
        {
            using var scope = _scopeFactory.CreateScope();
            var repoScoped = scope.ServiceProvider.GetRequiredService<IUserInfractionRepository>();
            var hub = scope.ServiceProvider.GetRequiredService<IHubContext<MultasHub>>();

            // Obtener el servicio de recordatorios
            var reminderService = scope.ServiceProvider.GetRequiredService<ReminderEmailAppService>();

            entity.StatusCollection = nuevoEstado;
            await repoScoped.UpdateAsync(entity);

            bool debeCancelar = nuevoEstado >= EstadoCobro.CobroCoactivo ||
                                entity.stateInfraction == EstadoMulta.ConAcuerdoPago;

            if (debeCancelar)
            {
                string razon = (entity.stateInfraction == EstadoMulta.ConAcuerdoPago)
                               ? "Acuerdo de Pago"
                               : nuevoEstado.ToString();

                _logger.LogWarning($"🛑 Multa #{entity.id} avanzado/modificado a {razon}. Cancelando recordatorios programados...");

                // **LLAMADA CRÍTICA:** Ejecutar la cancelación
                await reminderService.CancelarRecordatoriosPendientesAsync(entity.id);

                _logger.LogInformation($"✅ Cancelación de recordatorios terminada para multa #{entity.id}.");
            }

            // 3. DTO y Notificación
            var dto = _mapper.Map<UserInfractionDto>(entity);

            // Notificar a Angular en tiempo real
            await hub.Clients.All.SendAsync("MultaUpdated", dto);

            _logger.LogInformation($"📡 Notificación SignalR enviada para multa #{dto.id}, nuevo estado: {dto.StatusCollection}");
        }

        public async Task CancelarRecordatoriosPendientesAsync(int infractionId)
        {
            _logger.LogInformation($"🗑️ Intentando cancelar recordatorios pendientes para la infracción #{infractionId}...");

            var estadosACancelar = new[]
            {
        EstadoCobro.prejuridico3Dias,
        EstadoCobro.prejuridico15Dias,
        EstadoCobro.prejuridico25Dias,
        EstadoCobro.CobroJuridico,
        EstadoCobro.CobroCoactivo
    };

            foreach (var status in estadosACancelar)
            {
                string jobId = $"Infraction_{infractionId}_Status_{status}";
                await _scheduler.CancelJobAsync(jobId);
                _logger.LogWarning($"✅ Tarea de recordatorio cancelada para {status} ({jobId}).");
            }

            // 🔒 Limpiar del diccionario de programados
            _multasProgramadas.TryRemove(infractionId, out _);
            _logger.LogInformation($"🧹 Multa #{infractionId} removida del registro de programación.");
        }
    }
}

