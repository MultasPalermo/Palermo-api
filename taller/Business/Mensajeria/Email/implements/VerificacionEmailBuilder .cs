using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Business.Mensajeria.Email.implements
{
    public class VerificacionEmailBuilder : IEmailContentBuilder
    {
        private readonly string _nombre;
        private readonly string _codigo;

        public VerificacionEmailBuilder(string codigo)
        {
            _codigo = codigo;
        }

        public string GetSubject() => "✨ Tu código de verificación";

        public string GetBody() =>
            $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin: 0; padding: 0; background-color: #f0f4f8; font-family: ""Inter"", -apple-system, BlinkMacSystemFont, ""Segoe UI"", sans-serif;'>
    <table width='100%' cellpadding='0' cellspacing='0' style='padding: 50px 15px;'>
        <tr>
            <td align='center'>
                <!-- Contenedor principal -->
                <table width='100%' style='max-width: 580px; background-color: #ffffff; border-radius: 24px; overflow: hidden; box-shadow: 0 8px 32px rgba(0,0,0,0.08);'>
                   
                    
                    <!-- Espacio superior -->
                    <tr>
                        <td style='padding: 45px 40px 30px 40px;'>
                            
                            <!-- Título principal -->
                            <h1 style='
                                color: #1a202c;
                                font-size: 26px;
                                font-weight: 700;
                                text-align: center;
                                margin: 0 0 15px 0;
                                line-height: 1.3;'>
                                Verificación de Seguridad
                            </h1>
                            
                            <!-- Saludo personalizado -->
                            <p style='
                                font-size: 16px;
                                color: #4a5568;
                                text-align: center;
                                margin: 0 0 35px 0;
                                line-height: 1.6;'>
                                Hola <strong style='color: #66BB6A;'></strong>, hemos generado un código de acceso único para ti
                            </p>
                            
                            <!-- Tarjeta del código -->
                            <table width='100%' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td align='center' style='padding: 0 0 35px 0;'>
                                        <div style='
                                            background: linear-gradient(135deg, #f0fff4 0%, #e6f9e9 100%);
                                            border: 2px dashed #66BB6A;
                                            border-radius: 16px;
                                            padding: 35px 20px;
                                            text-align: center;'>
                                            <p style='
                                                margin: 0 0 12px 0;
                                                font-size: 13px;
                                                text-transform: uppercase;
                                                letter-spacing: 1.5px;
                                                color: #66BB6A;
                                                font-weight: 600;'>
                                                Tu Código
                                            </p>
                                            <div style='
                                                font-size: 42px;
                                                font-weight: 800;
                                                color: #66BB6A;
                                                letter-spacing: 12px;
                                                font-family: ""Courier New"", monospace;
                                                text-shadow: 2px 2px 4px rgba(102, 187, 106, 0.1);'>
                                                {_codigo}
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            
                            <!-- Información en cards -->
                            <table width='100%' cellpadding='0' cellspacing='0' style='margin-bottom: 30px;'>
                                <tr>
                                    <td style='padding-right: 8px; width: 50%; vertical-align: top;'>
                                        <div style='
                                            background-color: #fff8e1;
                                            border-radius: 12px;
                                            padding: 18px;
                                            text-align: center;
                                            border: 1px solid #ffecb3;'>
                                            <div style='font-size: 24px; margin-bottom: 8px;'>⏰</div>
                                            <p style='margin: 0; font-size: 12px; color: #f57c00; font-weight: 600;'>
                                                EXPIRA EN
                                            </p>
                                            <p style='margin: 5px 0 0 0; font-size: 18px; color: #e65100; font-weight: 700;'>
                                                15 min
                                            </p>
                                        </div>
                                    </td>
                                    <td style='padding-left: 8px; width: 50%; vertical-align: top;'>
                                        <div style='
                                            background-color: #e8f5e9;
                                            border-radius: 12px;
                                            padding: 18px;
                                            text-align: center;
                                            border: 1px solid #c8e6c9;'>
                                            <div style='font-size: 24px; margin-bottom: 8px;'>🔒</div>
                                            <p style='margin: 0; font-size: 12px; color: #2e7d32; font-weight: 600;'>
                                                SEGURO
                                            </p>
                                            <p style='margin: 5px 0 0 0; font-size: 18px; color: #1b5e20; font-weight: 700;'>
                                                100%
                                            </p>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            
                            <!-- Divider -->
                            <div style='height: 1px; background: linear-gradient(90deg, transparent, #e2e8f0, transparent); margin: 30px 0;'></div>
                            
                            <!-- Instrucciones -->
                            <div style='text-align: left;'>
                                <p style='
                                    margin: 0 0 15px 0;
                                    font-size: 14px;
                                    color: #2d3748;
                                    font-weight: 600;'>
                                    📋 Instrucciones:
                                </p>
                                <ol style='
                                    margin: 0 0 25px 0;
                                    padding-left: 20px;
                                    font-size: 14px;
                                    color: #4a5568;
                                    line-height: 1.8;'>
                                    <li>Copia el código mostrado arriba</li>
                                    <li>Regresa a la aplicación</li>
                                    <li>Pega el código en el campo de verificación</li>
                                    <li>¡Listo! Tu cuenta estará verificada</li>
                                </ol>
                            </div>
                            
                            <!-- Advertencia de seguridad -->
                            <div style='
                                background: linear-gradient(135deg, #fff3e0 0%, #ffe0b2 100%);
                                border-left: 4px solid #ff9800;
                                border-radius: 8px;
                                padding: 16px 20px;
                                margin-bottom: 25px;'>
                                <p style='margin: 0 0 8px 0; font-size: 13px; color: #e65100; font-weight: 700;'>
                                    IMPORTANTE
                                </p>
                                <p style='margin: 0; font-size: 13px; color: #bf360c; line-height: 1.6;'>
                                    Si no reconoces esta actividad, ignora este correo. Nunca compartas este código con nadie.
                                </p>
                            </div>
                            
                        </td>
                    </tr>
                    
                </table>
                
                <!-- Espaciado inferior -->
                <table width='100%' style='max-width: 580px; margin-top: 20px;'>
                    <tr>
                        <td style='text-align: center;'>
                            <p style='font-size: 11px; color: #94a3b8; margin: 0;'>
                                Este mensaje fue enviado a tu correo porque solicitaste un código de verificación
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

        public IEnumerable<Attachment>? GetAttachments()
        {
            return null; // No hay adjuntos
        }
    }
}