using Business.Interfaces.PDF;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.EntitiesDto;
using Entity.DTOs.Default.InstallmentSchedule;
using Entity.DTOs.Select.Entities;
using Microsoft.Playwright;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using Template.Templates;

namespace Business.Services.PDF
{
    public class PdfService : IPdfGeneratorService
    {
        private static IPlaywright? _playwright;
        private static IBrowser? _browser;

        private async Task EnsureBrowserAsync()
        {
            if (_playwright == null)
                _playwright = await Playwright.CreateAsync();

            if (_browser == null)
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true
                });
        }


        public async Task<byte[]> GeneratePdfAsync(UserInfractionSelectDto dto)
        {
            var html = BuildHtml(dto);

            await EnsureBrowserAsync();

            var context = await _browser!.NewContextAsync(new() { ViewportSize = null });
            try
            {
                var page = await context.NewPageAsync();
                await page.EmulateMediaAsync(new() { Media = Media.Print });
                await page.SetContentAsync(html, new PageSetContentOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle,
                    Timeout = 10_000
                });

                return await page.PdfAsync(new PagePdfOptions
                {
                    Format = "A4",
                    PrintBackground = true,
                    Margin = new()
                    {
                        Top = "40px",
                        Bottom = "40px",
                        Left = "40px",
                        Right = "40px"
                    }
                });
            }
            finally
            {
                await context.CloseAsync();
            }
        }

        public async Task<byte[]> GeneratePaymentAgreementPdfAsync(PaymentAgreementSelectDto dto)
        {
            Console.WriteLine($"🧾 Cuotas en DTO: {dto.InstallmentSchedule?.Count}");

            var html = BuildPaymentAgreementHtml(dto);

            await EnsureBrowserAsync();

            var context = await _browser!.NewContextAsync(new() { ViewportSize = null });
            try
            {
                var page = await context.NewPageAsync();
                await page.EmulateMediaAsync(new() { Media = Media.Print });
                await page.SetContentAsync(html, new PageSetContentOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle,
                    Timeout = 10_000
                });

                return await page.PdfAsync(new PagePdfOptions
                {
                    Format = "A4",
                    PrintBackground = true,
                    Margin = new()
                    {
                        Top = "40px",
                        Bottom = "40px",
                        Left = "40px",
                        Right = "40px"
                    }
                });
            }
            finally
            {
                await context.CloseAsync();
            }
        }
        private static string BuildHtml(UserInfractionSelectDto dto)
        {
            var template = InspectoraTemplate.Html;

            // ✅ Ruta física absoluta de la imagen
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Template", "images", "marcaAgua.png");
            imagePath = Path.GetFullPath(imagePath);

            Console.WriteLine("Ruta imagen: " + imagePath);
            Console.WriteLine("Existe: " + File.Exists(imagePath));

            string imageBase64 = string.Empty;

            if (File.Exists(imagePath))
            {
                // ✅ Convertir la imagen a Base64
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                string base64String = Convert.ToBase64String(imageBytes);
                imageBase64 = $"data:image/png;base64,{base64String}";
            }
            else
            {
                Console.WriteLine("❌ No se encontró la imagen en la ruta especificada.");
            }

            // ✅ Construcción del HTML con los valores dinámicos
            var html = template
                .Replace("@WatermarkBase64", imageBase64)
                .Replace("@Expediente", HttpUtility.HtmlEncode(dto.id.ToString()))
                .Replace("@Fecha", dto.dateInfraction.ToString("dd 'de' MMMM 'de' yyyy"))
                .Replace("@InfractorNombre", HttpUtility.HtmlEncode($"{dto.firstName} {dto.lastName}"))
                .Replace("@InfractorCedula", HttpUtility.HtmlEncode(dto.documentNumber ?? ""))
                .Replace("@TipoInfraccion", HttpUtility.HtmlEncode(dto.typeInfractionName))
                .Replace("@DescripcionInfraccion", HttpUtility.HtmlEncode(dto.observations));

            // 🧪 Guardar HTML temporal para verificar si se ve la imagen
            var debugPath = Path.Combine(Directory.GetCurrentDirectory(), "debug_inspectora.html");
            File.WriteAllText(debugPath, html);
            Console.WriteLine($"🧩 HTML de depuración guardado en: {debugPath}");

            return html;
        }

        private static string BuildPaymentAgreementHtml(PaymentAgreementSelectDto dto)
        {
            var culture = new CultureInfo("es-CO");

            // ✅ Cargar la imagen del encabezado
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Template", "images", "marcaAgua.png");
            imagePath = Path.GetFullPath(imagePath);
            Console.WriteLine("Ruta imagen: " + imagePath);
            Console.WriteLine("Existe: " + File.Exists(imagePath));

            string imageBase64 = string.Empty;
            if (File.Exists(imagePath))
            {
                // ✅ Convertir la imagen a Base64
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                string base64String = Convert.ToBase64String(imageBytes);
                imageBase64 = $"data:image/png;base64,{base64String}";
            }
            else
            {
                Console.WriteLine("❌ No se encontró la imagen en la ruta especificada.");
            }

            // ✅ Verificar que el cronograma existe
            var schedule = dto.InstallmentSchedule ?? new List<InstallmentScheduleDto>();
            Console.WriteLine($"📊 DEBUG - Cuotas recibidas en BuildHtml: {schedule.Count}");

            var cuotasHtml = new StringBuilder();
            if (schedule.Any())
            {
                cuotasHtml.AppendLine("<h2>📅 Cronograma de Cuotas</h2>");
                cuotasHtml.AppendLine("<table>");
                cuotasHtml.AppendLine("<tr><th>#</th><th>Fecha de Pago</th><th>Valor Cuota</th><th>Saldo Restante</th></tr>");

                foreach (var cuota in schedule)
                {
                    cuotasHtml.AppendLine("<tr>");
                    cuotasHtml.AppendLine($"<td>{cuota.Number}</td>");
                    cuotasHtml.AppendLine($"<td>{cuota.PaymentDate:dd/MM/yyyy}</td>");
                    cuotasHtml.AppendLine($"<td>$ {cuota.Amount.ToString("N0", culture)}</td>");
                    cuotasHtml.AppendLine($"<td>$ {cuota.RemainingBalance.ToString("N0", culture)}</td>");
                    cuotasHtml.AppendLine("</tr>");
                }

                cuotasHtml.AppendLine("</table>");
            }
            else
            {
                cuotasHtml.AppendLine("<p style='color: red; font-weight: bold;'>⚠️ No se generó cronograma de cuotas.</p>");
            }

            // ✅ Generar el HTML final
            var html = PaymentAgreementTemplate.Html
                .Replace("@WatermarkBase64", imageBase64)
                .Replace("@Nombre", HttpUtility.HtmlEncode(dto.PersonName ?? "-"))
                .Replace("@Documento", HttpUtility.HtmlEncode(dto.DocumentNumber ?? "-"))
                .Replace("@TipoDocumento", HttpUtility.HtmlEncode(dto.DocumentType ?? "-"))
                .Replace("@Direccion", HttpUtility.HtmlEncode(dto.address ?? "-"))
                .Replace("@Barrio", HttpUtility.HtmlEncode(dto.Neighborhood ?? "-"))
                .Replace("@Telefono", HttpUtility.HtmlEncode(dto.PhoneNumber ?? "-"))
                .Replace("@Correo", HttpUtility.HtmlEncode(dto.Email ?? "-"))
                .Replace("@FechaInicio", dto.AgreementStart.ToString("dd/MM/yyyy"))
                .Replace("@FechaFin", dto.AgreementEnd.ToString("dd/MM/yyyy"))
                .Replace("@ExpedicionCedula", dto.expeditionCedula.ToString("dd/MM/yyyy"))
                .Replace("@MetodoPago", HttpUtility.HtmlEncode(dto.PaymentMethod ?? "-"))
                .Replace("@FrecuenciaPago", HttpUtility.HtmlEncode(dto.FrequencyPayment ?? "-"))
                .Replace("@TipoInfraccion", HttpUtility.HtmlEncode(dto.TypeFine ?? "-"))
                .Replace("@Infraccion", HttpUtility.HtmlEncode(dto.Infringement ?? "-"))
                .Replace("@Descripcion", HttpUtility.HtmlEncode(dto.Infringement ?? "-"))
                .Replace("@ValorSMDLV", "$ " + dto.ValorSMDLV.ToString("N0", culture))
                .Replace("@MontoBase", "$ " + dto.BaseAmount.ToString("N0", culture))
                .Replace("@Intereses", "$ " + dto.AccruedInterest.ToString("N0", culture))
                .Replace("@SaldoPendiente", "$ " + dto.OutstandingAmount.ToString("N0", culture))
                .Replace("@Cuotas", dto.Installments?.ToString() ?? "-")
                .Replace("@ValorCuota", dto.MonthlyFee.HasValue ? "$ " + dto.MonthlyFee.Value.ToString("N0", culture) : "-")
                .Replace("@Estado", dto.IsPaid ? "✅ Pagado" : "⏳ Pendiente")
                .Replace("@Coactivo", dto.IsCoactive
                    ? $"<p><strong>Proceso coactivo desde:</strong> {dto.CoactiveActivatedOn:dd/MM/yyyy}</p>"
                    : "")
                .Replace("@UltimoInteres", dto.LastInterestAppliedOn.HasValue
                    ? $"<p><strong>Último cálculo de interés:</strong> {dto.LastInterestAppliedOn:dd/MM/yyyy}</p>"
                    : "")
                .Replace("@TablaCuotas", cuotasHtml.ToString());

            // 🧪 Guardar HTML temporal para verificar si se ve la imagen
            var debugPath = Path.Combine(Directory.GetCurrentDirectory(), "debug_payment_agreement.html");
            File.WriteAllText(debugPath, html);
            Console.WriteLine($"🧩 HTML de depuración guardado en: {debugPath}");

            return html;
        }
    }
}
