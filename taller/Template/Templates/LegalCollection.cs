using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Templates
{
    using System;

    namespace Template.Templates
    {
        public static class LegalCollection
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
      margin-top: 40px;
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
      <h2>Aviso de traslado a cobro jurídico</h2>
    </div>

    <div class='content'>
      <p><strong>Entidad:</strong> Alcaldía Municipal de Palermo – Secretaría de Hacienda<br/>
         <strong>Dependencia:</strong> Tesorería / Área de Cobro Persuasivo<br/>
         <strong>Referencia:</strong> Multa por infracción al Código Nacional de Seguridad y Convivencia (Ley 1801 de 2016)<br/>
         <strong>Expediente:</strong> {{expediente}}<br/>

      <p><strong>Señor(a):</strong> {{nombre_completo}}<br/>
         <strong>Identificación:</strong> {{cedula}}<br/>
         <strong>Correo electrónico registrado:</strong> {{correo}}</p>

      <p>Palermo, Huila, {{fecha_emision}}</p>

      <p>Cordial saludo,</p>

      <p>La Secretaría de Hacienda Municipal informa que usted cuenta con una obligación pendiente derivada, por medio de la cual se impuso una multa tipo {{tipo_multa}} conforme a la Ley 1801 de 2016.</p>

      <p>De acuerdo con el registro institucional, el plazo de treinta (30) días calendario otorgado para el pago voluntario venció sin que se registrara pago o solicitud válida.</p>

      <p>Por lo anterior, esta comunicación tiene como finalidad informarle que, si dentro de los cinco (5) días hábiles siguientes a la recepción de este correo usted no cancela la obligación o no solicita acuerdo de pago, su expediente será trasladado al área jurídica para el inicio del proceso de cobro coactivo.</p>

      <p><strong>Posibles consecuencias del cobro jurídico:</strong></p>
      <ul>
        <li>Mandamiento de pago.</li>
        <li>Embargo de cuentas bancarias.</li>
        <li>Retención de salarios.</li>
        <li>Medidas cautelares sobre bienes.</li>
        <li>Registro de la obligación en sistemas de información fiscal municipal.</li>
      </ul>

      <p><strong>Valor pendiente de pago:</strong></p>
      <ul>
        <li>Valor base: ${{valor_multa}}</li>
        <li>Total adeudado: ${{total}}</li>
      </ul>

      <p><strong>Opciones antes del cobro jurídico:</strong></p>
      <ol>
        <li>Pagar la totalidad de la obligación.</li>
        <li>Solicitar acuerdo de pago a la alcaldia de palermo-huila</li>
      </ol>

      <p>Una vez el expediente entre a cobro jurídico, no serán posibles acuerdos en etapa persuasiva.</p>
    </div>

    <div class='firma'>
         Tesorero Municipal<br/>
         Secretaría de Hacienda – Alcaldía Municipal de Palermo</p>
    </div>
  </div>
</body>
</html>";
        }
    }

}
