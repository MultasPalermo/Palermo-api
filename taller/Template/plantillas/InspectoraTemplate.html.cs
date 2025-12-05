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
      text-align: justify;
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
      margin: 150px 50px 100px 50px;
      position: relative;
      z-index: 1;
    }
    .titulo h2 {
      margin: 0;
      font-size: 14pt;
    }

    .content p {
      margin-bottom: 10px;
    }

    strong {
      font-weight: bold;
    }
  </style>
</head>

<body>

  <!-- Encabezado -->
  <img src=""@WatermarkBase64"" class=""header-watermark"" alt=""Encabezado Alcaldía de Palermo"" />

  <div class=""page-content"">

    <div class=""content"">

      <p>113.12.02. – 1068</p>

      <p><strong>PARA:</strong><strong> ALEJANDRO GOMEZ MONTENEGRO</strong><br>
         Secretario de Hacienda Municipal.</p>

      <p><strong>DE:</strong><strong>ADRIANA YINETH FRANCO GARCIA</strong><br>
         Inspectora de Policía Municipal</p>

      <p><strong>ASUNTO:</strong><strong>ANCIÓN PECUNIARIA - EXPEDIENTE</strong><br>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;No. <strong>@Expediente</strong></p>

      <p><strong>FECHA:</strong> @Fecha</p>

      <p>Cordial saludo,</p>

      <p>
        Comedidamente le informo que la fecha  <strong>@Fecha</strong>, la Policía Nacional adscrita al Municipio de Palermo,
        impuso orden de comparendo número <strong>@Expediente</strong>, a <strong>@InfractorNombre</strong> identificado con cédula de ciudadanía
        N° <strong>@InfractorCedula</strong>, por Comportamientos contrarios al cuidado e integridad del espacio público, establecido en el
        Artículo 35 numeral 2, de la ley 1801 de 2016, imponiéndose una <strong>@TipoInfraccion</strong>: 
        <strong>@numer_smldv</strong><strong> salarios mínimos diarios legales vigentes (smdlv).</strong> Que <strong>@InfractorNombre</strong> no objeto y tampoco
        conmuto ante la Inspección de Policía dentro del término legal establecido en el parágrafo único del artículo 180.
      </p>

      <p>
        En este orden de ideas, de conformidad con el principio de celeridad y el artículo 223A de la ley 1801 de 2016
        no podrá iniciarse el proceso verbal abreviado, por cuanto se pierde la oportunidad legal establecida en el
        inciso 7 parágrafo único del artículo 180 de la misma ley.
      </p>

      <p>
        Igualmente, conforme al literal e del artículo 223A se produce la firmeza de la multa señalada en orden de comparendo,
        no objetada, una vez vencidos los cinco (05) días posteriores a la expedición de la orden, la multa queda en firme,
        pudiéndose iniciar el cobro coactivo, por lo tanto, esta inspección de policía no expedirá resolución administrativa.
      </p>
    </div>

    <p>Cordialmente.</p>

    <p style=""text-align:center;"">
      <strong>ADRIANA YINETH FRANCO GARCIA</strong><br>
      Inspectora de Policía Municipal
    </p>

  </div>
</body>
</html>";
    }
}
