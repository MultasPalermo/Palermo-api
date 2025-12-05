using System;

namespace Template.Templates
{
    public static class ThreeDayReminderTemplate
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
      max-height: 150px;
      z-index: -1;
      opacity: 1;
    }

    .page-content {
      margin: 120px 60px 100px 60px;
      position: relative;
      z-index: 1;
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
      .page-content {
        page-break-inside: avoid;
      }
    }
  </style>
</head>
<body>
  <img src='@WatermarkBase64' class='header-watermark' alt='Marca de agua' />

  <div class='page-content'>
    <div class='content'>
      <p><strong>Municipio de Palermo - Huila</strong><br/>
      <strong>Secretaría de Hacienda Municipal</strong><br/>
      <strong>Fecha:</strong> {{fecha_emision}}</p>

      <p><strong>ASUNTO:</strong> Recordatorio de pago de multa</p>

      <p><strong>Respetado(a):</strong> {{nombre_completo}}</p>

      <p>
        De acuerdo con el Código Nacional de Seguridad y Convivencia Ciudadana (Ley 1801 de 2016)
        mediante la cual se impuso una multa tipo <strong>{{tipo_multa}}</strong>, 
        le recordamos que el término para realizar el pago voluntario se encuentra próximo a vencer.
      </p>

      <p>
        Evite el inicio del proceso de cobro coactivo efectuando el pago dentro de los próximos días. 
        Para mayor información puede comunicarse con la Secretaría de Hacienda Municipal.
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
