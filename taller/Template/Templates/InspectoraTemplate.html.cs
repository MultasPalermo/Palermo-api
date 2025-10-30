using System;
namespace Template.Templates
{
    public static class InspectoraTemplate
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
      margin: 0;
      padding: 0;
      position: relative;
      text-align: justify;
    }
    
    /* ✅ Encabezado fijo en la parte superior */
    .header-watermark {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: auto;
      max-height: 150px;
      z-index: -1;
      opacity: 1;
    }
    
    /* ✅ Pie de página fijo en la parte inferior (opcional) */
    .footer-watermark {
      position: fixed;
      bottom: 0;
      left: 0;
      width: 100%;
      height: auto;
      max-height: 100px;
      z-index: -1;
      opacity: 1;
    }
    
    /* Contenedor general del texto con márgenes para evitar superposición */
    .page-content {
      margin: 170px 70px 120px 70px; /* Ajusta estos valores según la altura de tus imágenes */
      position: relative;
      z-index: 1;
    }
    
    .titulo {
      text-align: center;
      font-weight: bold;
      text-transform: uppercase;
      margin-bottom: 30px;
    }
    
    .titulo h2 {
      margin: 0;
      font-size: 14pt;
    }
    
    .content p {
      margin-bottom: 10px;
    }
    
    .footer {
      margin-top: 80px;
      text-align: center;
      font-style: italic;
    }
    
    strong {
      font-weight: bold;
    }
    
    /* ✅ Estilos para impresión/PDF */
    @media print {
      .header-watermark, .footer-watermark {
        position: fixed;
      }
      .page-content {
        page-break-inside: avoid;
      }
    }
  </style>
</head>
<body>
  <!-- ✅ Encabezado institucional fijo en la parte superior -->
  <img src=""@WatermarkBase64"" class=""header-watermark"" alt=""Encabezado Alcaldía de Palermo"" />
  
  <!-- ✅ Si tienes imagen de pie de página, descomenta esta línea -->
  <!-- <img src=""@FooterWatermarkBase64"" class=""footer-watermark"" alt=""Pie de página"" /> -->
  
  <div class=""page-content"">
    <div class=""titulo"">
      <h2>INFORME DE COMPARENDO</h2>
    </div>
    <div class=""content"">
      <p><strong>Expediente N° @Expediente</strong></p>
      <p><strong>PARA:</strong> Secretario de Hacienda Municipal</p>
      <p><strong>DE:</strong> ADRIANA YINETH FRANCO GARCIA<br>Inspectora de Policía Municipal</p>
      <p><strong>FECHA:</strong> @Fecha</p>
      <p>Cordial saludo,</p>
      <p>
        Comedidamente le informo que el día @Fecha, la Policía Nacional adscrita al Municipio de Palermo,
        impuso orden de comparendo número <strong>@Expediente</strong> a 
        <strong>@InfractorNombre</strong>, identificado con cédula de ciudadanía N° 
        <strong>@InfractorCedula</strong>, por <em>@DescripcionInfraccion</em>,
        imponiéndose una Multa Tipo <strong>@TipoInfraccion</strong>.
      </p>
    </div>
    <div class=""footer"">
      <p>Cordialmente,</p>
      <br><br><br>
      <p><strong>ADRIANA YINETH FRANCO GARCIA</strong><br>
      Inspectora de Policía Municipal</p>
    </div>
  </div>
</body>
</html>";
    }
}