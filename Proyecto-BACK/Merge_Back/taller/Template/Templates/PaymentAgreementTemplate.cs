using System;
namespace Template.Templates
{
    public static class PaymentAgreementTemplate
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
      margin: 0;
      padding: 0;
      position: relative;
    }
    
    /* ✅ Encabezado en la parte superior - solo primera página */
    .header-watermark {
      display: block;
      width: 100%;
      height: auto;
      max-height: 150px;
      margin: 0;
      padding: 0;
    }
    
    /* Contenedor principal con márgenes normales */
    .page-content {
      margin: 20px 40px 40px 40px;
      position: relative;
    }
    
    h1 {
      text-align: center;
      color: #2c3e50;
      border-bottom: 2px solid #4CAF50;
      padding-bottom: 10px;
      margin-bottom: 20px;
    }
    
    h2 {
      color: #2c3e50;
      margin-top: 25px;
      margin-bottom: 15px;
    }
    
    .section {
      margin-top: 20px;
    }
    
    .label {
      font-weight: bold;
      color: #555;
    }
    
    .value {
      margin-left: 5px;
    }
    
    table {
      width: 100%;
      border-collapse: collapse;
      margin-top: 15px;
    }
    
    table, th, td {
      border: 1px solid #ccc;
    }
    
    th, td {
      padding: 8px;
      text-align: left;
    }
    
    th {
      background-color: #f4f4f4;
    }
    
    .footer {
      margin-top: 40px;
      text-align: center;
      font-style: italic;
    }
    
    /* ✅ Estilos para impresión/PDF */
    @media print {
      .header-watermark {
        display: block;
        page-break-after: avoid;
      }
      .page-content {
        page-break-inside: avoid;
      }
    }
  </style>
</head>
<body>
  <!-- ✅ Encabezado institucional en la parte superior -->
  <img src=""@WatermarkBase64"" class=""header-watermark"" alt=""Encabezado"" />
  
  <div class=""page-content"">
    <h1>📑 Acuerdo de Pago</h1>
    
    <div class=""section"">
      <span class=""label"">Nombre:</span> <span class=""value"">@Nombre</span><br/>
      <span class=""label"">Documento:</span> <span class=""value"">@Documento</span><br/>
      <span class=""label"">Tipo Documento:</span> <span class=""value"">@TipoDocumento</span><br/>
      <span class=""label"">Dirección:</span> <span class=""value"">@Direccion</span><br/>
      <span class=""label"">Barrio:</span> <span class=""value"">@Barrio</span><br/>
      <span class=""label"">Teléfono:</span> <span class=""value"">@Telefono</span><br/>
      <span class=""label"">Correo:</span> <span class=""value"">@Correo</span><br/>
    </div>
    
    <div class=""section"">
      <span class=""label"">Inicio del acuerdo:</span> <span class=""value"">@FechaInicio</span><br/>
      <span class=""label"">Fin del acuerdo:</span> <span class=""value"">@FechaFin</span><br/>
      <span class=""label"">Expedición Cédula:</span> <span class=""value"">@ExpedicionCedula</span><br/>
      <span class=""label"">Método de pago:</span> <span class=""value"">@MetodoPago</span><br/>
      <span class=""label"">Frecuencia de pago:</span> <span class=""value"">@FrecuenciaPago</span><br/>
      <span class=""label"">Tipo infracción:</span> <span class=""value"">@TipoInfraccion</span><br/>
      <span class=""label"">Infracción:</span> <span class=""value"">@Infraccion</span><br/>
      <span class=""label"">Descripción:</span> <span class=""value"">@Descripcion</span>
    </div>
    
    <h2>💰 Información financiera</h2>
    <table>
      <tr>
        <th>Monto Base</th>
        <th>Intereses</th>
        <th>Saldo Pendiente</th>
        <th>Cuotas</th>
        <th>Valor Cuota</th>
      </tr>
      <tr>
        <td>@MontoBase</td>
        <td>@Intereses</td>
        <td>@SaldoPendiente</td>
        <td>@Cuotas</td>
        <td>@ValorCuota</td>
      </tr>
    </table>
    
    <!-- Aquí insertamos dinámicamente el cronograma -->
@TablaCuotas
    
    <div class=""section"">
      <p><span class=""label"">Estado:</span> @Estado</p>
      @Coactivo
      @UltimoInteres
    </div>
    
    <div class=""footer"">
      <p>Sistema de Gestión de Multas</p>
    </div>
  </div>
</body>
</html>";
    }
}