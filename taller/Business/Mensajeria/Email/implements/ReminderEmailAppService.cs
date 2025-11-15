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
        private readonly INotificationSettingServices _notificationSettingService;
        private readonly IUserInfractionRepository _repo;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<MultasHub> _hub;

        public ReminderEmailAppService(
            IServiceEmail emailService,
            IPdfGeneratorService pdfService,
            EmailScheduler scheduler,
            ILogger<ReminderEmailAppService> logger,
            INotificationSettingServices notificationSettingService,
            IMapper mapper,
            IUserInfractionRepository repo,
            IServiceScopeFactory scopeFactory,
            IHubContext<MultasHub> hub)
        {
            _emailService = emailService;
            _pdfService = pdfService;
            _scheduler = scheduler;
            _logger = logger;
            _notificationSettingService = notificationSettingService;
            _repo = repo;
            _mapper = mapper;
            _scopeFactory = scopeFactory;
            _hub = hub;
        }


        /// <summary>
        /// programa los recordatorios reales basados en la fecha de infracción.
        /// </summary>
        public async Task ProgramarRecordatoriosAsync(UserInfractionSelectDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("❌ dto nulo. no se pueden programar recordatorios.");
                return;
            }

            if (string.IsNullOrWhiteSpace(dto.userEmail))
            {
                _logger.LogWarning($"⚠️ La infracción #{dto.id} no tiene correo asignado.");
                return;
            }

            if (dto.dateInfraction == default)
            {
                _logger.LogWarning($"⚠️ La infracción #{dto.id} no tiene fecha válida.");
                return;
            }

            _logger.LogInformation($"📅 Programando recordatorios para infracción #{dto.id} ({dto.userEmail})...");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var settingService = scope.ServiceProvider.GetRequiredService<INotificationSettingServices>();

                var settings = (await settingService.GetAllAsync())?.ToList();

                if (settings == null || settings.Count < 5)
                {
                    _logger.LogWarning("⚠️ No están configurados los recordatorios 1 a 5.");
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
                    return;
                }

                string timeUnit = r1.TimeUnit?.ToUpper() ?? "DAYS";
                bool usarSegundos = timeUnit == "SECONDS";

                _logger.LogWarning($"⏱ Modo utilizado: {(usarSegundos ? "SEGUNDOS (pruebas)" : "DÍAS (normal)")}");

                int v1 = r1.Days;
                int v2 = r2.Days;
                int v3 = r3.Days;
                int v4 = r4.Days;
                int v5 = r5.Days;

                _logger.LogInformation($"🔧 Configuración → R1: {v1} {timeUnit}, R2: {v2} {timeUnit}, R3: {v3} {timeUnit}, R4: {v4} {timeUnit}, R5: {v5} {timeUnit}");

                DateTime CalcFecha(int val) => usarSegundos ? dto.dateInfraction.AddSeconds(val) : dto.dateInfraction.AddDays(val);
                TimeSpan CalcDelay(DateTime target) => target - DateTime.Now;
                string Etiqueta(int val) => usarSegundos ? $"{val} segundos" : $"{val} días";

                var fechas = new[]
                {
            (v1, CalcFecha(v1), Etiqueta(v1)),
            (v2, CalcFecha(v2), Etiqueta(v2)),
            (v3, CalcFecha(v3), Etiqueta(v3)),
            (v4, CalcFecha(v4), Etiqueta(v4)),
            (v5, CalcFecha(v5), Etiqueta(v5))
        };

                foreach (var (valor, fechaEnvio, etiqueta) in fechas)
                {
                    var delay = usarSegundos ? TimeSpan.FromSeconds(valor) : CalcDelay(fechaEnvio);

                    if (delay <= TimeSpan.Zero)
                    {
                        _logger.LogWarning($"⚠️ Ya pasó la fecha del recordatorio ({etiqueta}) para #{dto.id}. No se programará.");
                        continue;
                    }

                    await ProgramarEnvioAsync(dto, valor, etiqueta, delay);
                    _logger.LogInformation($"⏳ Recordatorio programado: {etiqueta} → En {delay}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al programar recordatorios para infracción #{dto.id}");
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



        private async Task ProgramarEnvioAsync(UserInfractionSelectDto dto, int dias, string etiqueta, TimeSpan delay)
        {
            await _scheduler.ScheduleEmailAsync(async () =>
            {
                try
                {
                    _logger.LogInformation($"🚀 Enviando recordatorio de {etiqueta} a {dto.userEmail}...");

                    using var scope = _scopeFactory.CreateScope();
                    var repoScoped = scope.ServiceProvider.GetRequiredService<IUserInfractionRepository>();

                    var entity = await repoScoped.GetByIdAsync(dto.id);
                    if (entity == null)
                    {
                        _logger.LogWarning($"⚠ La infracción #{dto.id} no existe en BD.");
                        return;
                    }

                    // ----- ACTUALIZA ESTADO -----
                    entity.StatusCollection = entity.StatusCollection switch
                    {
                        EstadoCobro.CobroPrejuridico => EstadoCobro.prejuridico3Dias,
                        EstadoCobro.prejuridico3Dias => EstadoCobro.prejuridico15Dias,
                        EstadoCobro.prejuridico15Dias => EstadoCobro.prejuridico25Dias,
                        EstadoCobro.prejuridico25Dias => EstadoCobro.CobroJuridico,
                        EstadoCobro.CobroJuridico => EstadoCobro.CobroCoactivo,
                        EstadoCobro.CobroCoactivo => EstadoCobro.CobroCoactivo,
                        _ => EstadoCobro.CobroPrejuridico
                    };

                    await repoScoped.UpdateAsync(entity);

                    dto.StatusCollection = entity.StatusCollection.ToString();
                    var estado = entity.StatusCollection;

                    _logger.LogInformation($"📌 Estado actualizado → {estado}");

                    // ----- CONTENIDO DEL CORREO -----
                    var (subject, body) = ObtenerContenidoCorreo(dto, estado);

                    // ----- OBTENER ID DE RECORDATORIO -----
                    int reminderId = ObtenerRecordatorioIdPorEstado(estado);

                    // ----- GENERAR PDF SI CORRESPONDE -----
                    byte[]? pdfBytes = null;
                    if (reminderId > 0)
                    {
                        pdfBytes = await _pdfService.GenerateReminderPdfAsync(dto, reminderId);
                    }

                    // ----- ENVIAR CORREO SEGÚN SI HAY PDF -----
                    if (pdfBytes != null)
                    {
                        _logger.LogInformation($"📎 Adjuntando PDF para recordatorio {reminderId}");

                        var attachment = new Attachment(new MemoryStream(pdfBytes),
                            $"Recordatorio_{dto.id}_R{reminderId}.pdf");

                        await _emailService.SendEmailAsync(
                            dto.userEmail,
                            subject,
                            body,
                            new List<Attachment> { attachment }
                        );
                    }
                    else
                    {
                        _logger.LogInformation($"📨 Envío sin PDF (recordatorio {reminderId})");

                        await _emailService.SendEmailAsync(dto.userEmail, subject, body);
                    }


                    _logger.LogInformation($"✅ Recordatorio enviado correctamente a {dto.userEmail}");

                    // SignalR
                    await _hub.Clients.All.SendAsync(
                        "ReceiveStatusUpdate",
                        dto.id,
                        dto.StatusCollection
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"❌ Error al enviar recordatorio de {etiqueta} a {dto.userEmail}");
                }

            }, delay);
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

            // Actualizar estado
            entity.StatusCollection = nuevoEstado;
            await repoScoped.UpdateAsync(entity);

            // DTO para enviar al front
            var dto = _mapper.Map<UserInfractionDto>(entity);

            // 🚀 Notificar a Angular en tiempo real
            await hub.Clients.All.SendAsync("MultaUpdated", dto);

            _logger.LogInformation($"📡 Notificación SignalR enviada para multa #{dto.id}, nuevo estado: {dto.StatusCollection}");
        }
    }
}
