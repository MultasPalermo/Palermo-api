using System;

namespace Template.Templates
{
    public static class TwentyFiveDayReminderTemplate
    {
        public static readonly string Html = @"
<!DOCTYPE html>
<html lang=""es"">
<head>
  <meta charset=""UTF-8"">
  <style>
    body {
      font-family: 'Times New Roman', serif;
      font-size: 12pt;
      line-height: 1.5;
      color: #000;
      text-align: justify;
      margin: 0;
      padding: 0;
      position: relative;
    }

    .header-watermark {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      opacity: 0.1;
      z-index: -1;
    }

    .page-content {
      margin: 120px 60px 100px 60px;
      position: relative;
      z-index: 1;
    }

    .titulo {
      text-align: center;
      text-transform: uppercase;
      font-weight: bold;
      margin-bottom: 30px;
      font-size: 14pt;
    }

    .content p {
      margin-bottom: 10px;
    }

    .firma {
      margin-top: 80px;
      text-align: left;
      line-height: 1.2;
    }

    .firma strong {
      display: block;
    }

    @media print {
      .header-watermark {
        position: fixed;
      }
      .page-content {
        page-break-inside: avoid;
      }
    }
  </style>
</head>
<body>
  <img src='@WatermarkBase64' class='header-watermark' alt='Marca de agua' />

  <div class='page-content'>
    <div class='titulo'>
      <h2>Tercer recordatorio persuasivo de pago</h2>
    </div>

    <div class='content'>
      <p><strong>Municipio de Palermo - Huila</strong><br/>
      <strong>Secretaría de Hacienda Municipal</strong><br/>
      <strong>Fecha:</strong> @FechaActual</p>

      <p><strong>ASUNTO:</strong> Último aviso antes de inicio de cobro coactivo - Resolución N.° <strong>@NumeroResolucion</strong></p>

      <p><strong>Respetado(a):</strong> @NombreCompleto</p>

      <p>
        Transcurridos veinticinco (25) días sin registrarse pago de la multa establecida mediante Resolución N.° 
        <strong>@NumeroResolucion</strong>, le informamos que de no realizar el pago o suscribir un acuerdo dentro de 
        los próximos cinco (5) días, se dará inicio al proceso de cobro coactivo.
      </p>

      <p>
        Evite sanciones adicionales y costos procesales pagando voluntariamente.
      </p>
    </div>

    <div class='firma'>
      <p><strong>Atentamente,</strong><br/>
      Secretaría de Hacienda Municipal<br/>
      Municipio de Palermo - Huila</p>
    </div>
  </div>
</body>
</html>";
    }
}
