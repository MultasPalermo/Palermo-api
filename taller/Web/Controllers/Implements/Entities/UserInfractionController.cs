        // ===========================================================
        // 📄 Descargar recordatorio de 25 días
        // ===========================================================
        [HttpGet("{id}/pdf/25dias")]
        public async Task<IActionResult> DownloadReminder25DaysPdf(int id)
        {
            var userInfraction = await _service.GetByIdAsyncPdf(id);
            if (userInfraction == null)
                return NotFound(new { message = $"No se encontró una infracción con id {id}" });

            var pdfBytes = await _pdfService.GenerateReminderPdfAsync(userInfraction, 25);
            return File(pdfBytes, "application/pdf", $"Recordatorio_25dias_{userInfraction.id}.pdf");
        }

        [HttpPost("simulate-interest")]
        public async Task<IActionResult> SimulateInterest(
     [FromQuery] int idUserInfraction,
     [FromQuery] int days)
        {
            if (idUserInfraction <= 0)
                return BadRequest("el id del userInfraction es obligatorio.");

            if (days < 0)
                return BadRequest("los días deben ser positivos.");

            // fecha simulada
            var simulatedDate = DateTime.UtcNow.AddDays(days);

            bool updated = await _userInfractionServices
                .ApplyInterestToSingleInfractionAsync(idUserInfraction, simulatedDate);

            return Ok(new
            {
                isSuccess = updated,
                idUserInfraction,
                simulatedDate
            });
        }





        //[HttpPost("test-email")]
        //public async Task<IActionResult> TestEmail([FromServices] ReminderEmailAppService service)
        //{
        //    var dto = new UserInfractionSelectDto
        //    {
        //        id = 12345,
        //        firstName = "Camilo",
        //        lastName = "Andrés",
        //        userEmail = "camiloandreslosada801@gmail.com",
        //        dateInfraction = DateTime.Now.AddDays(-5),
        //        amountToPay = 250000
        //    };

        //    await service.ProgramarRecordatoriosAsync(dto);
        //    return Ok("📨 Recordatorios programados (ver logs para el resultado).");
        //}


        //[HttpPost("test-send-email")]
        //public async Task<IActionResult> TestSendEmail([FromBody] UserInfractionDto dto)
        //{
        //    try
        //    {
        //        // 1️⃣ Crear la multa y obtener el DTO resultante con ID generado
        //        var createdDto = await _service.CreateAsync(dto);

        //        // 2️⃣ Obtener la multa completa con todos los datos necesarios para PDF
        //        var created = await _service.GetByIdAsyncPdf(createdDto.id);
        //        if (created == null)
        //            return NotFound(new { message = "No se pudo obtener la infracción creada." });

        //        // 3️⃣ Obtener el usuario (solo para email)
        //        var user = await _users.GetByIdAsync(dto.userId);
        //        if (user == null || string.IsNullOrEmpty(user.email))
        //            return NotFound(new { message = "Usuario no encontrado o sin email." });

        //        // 4️⃣ Generar PDF
        //        var pdfBytes = await _pdfService.GeneratePdfAsync(created);
        //        if (pdfBytes == null || pdfBytes.Length == 0)
        //            return StatusCode(500, new { message = "Error generando el PDF." });

        //        // 5️⃣ Construir y enviar email
        //        var builder = new InfraccionEmailBuilder(created, pdfBytes);

        //        using var scope = _scopeFactory.CreateScope();
        //        var emailService = scope.ServiceProvider.GetRequiredService<IServiceEmail>();

        //        await emailService.SendEmailAsync(
        //            user.email,
        //            builder.GetSubject(),
        //            builder.GetBody(),
        //            builder.GetAttachments()?.ToList()
        //        );

        //        // ✅ Devuelve la multa creada para verificación en Postman
        //        return Ok(created);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creando la infracción y enviando correo");
        //        return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        //    }
        //}


    }

}

