namespace Templates.Template
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
      font-family: Arial, sans-serif;
      font-size: 12pt;
      line-height: 1.5;
      color: #000;
      margin: 40px;
    }

    h1, h2 {
      text-align: center;
      text-transform: uppercase;
    }

    .header {
      margin-bottom: 30px;
    }

    .section {
      margin-top: 20px;
    }

    .footer {
      margin-top: 60px;
      text-align: center;
      font-style: italic;
    }
  </style>
</head>
<body>
  <div class=""header"">
    <p><strong>@Expediente</strong></p>
    <p>PARA: Secretario de Hacienda Municipal</p>
    <p>DE: ADRIANA YINETH FRANCO GARCIA<br>Inspectora de Policía Municipal</p>
    <p>ASUNTO: @Asunto</p>
    <p>FECHA: @Fecha</p>
  </div>

  <div class=""section"">
    <p>Cordial saludo,</p>

    <p>Comedidamente le informo que el día @Fecha, la Policía Nacional adscrita al Municipio de Palermo, 
    impuso orden de comparendo número <strong>@Expediente</strong>, a <strong>@InfractorNombre</strong>, 
    identificado con cédula de ciudadanía N° <strong>@InfractorCedula</strong>, por 
    <em>@DescripcionInfraccion</em>, establecido en el Artículo <strong>@Articulo</strong>, de la ley 1801 de 2016, 
    imponiéndose una Multa Tipo <strong>@TipoInfraccion</strong> : <strong>@SalariosMinimos</strong> salarios mínimos 
    diarios legales vigentes (smdlv), equivalente a <strong>@ValorMultaPesos</strong>.</p>

    <p>@Mensaje</p>
  </div>

  <div class=""footer"">
    <p>Cordialmente,</p>
    <br><br>
    <p><strong>ADRIANA YINETH FRANCO GARCIA</strong><br>
    Inspectora de Policía Municipal</p>
  </div>
</body>
</html>";
    }
}

    

