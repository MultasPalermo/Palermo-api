using System;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.EntitiesDataInit
{
    public static class InfractionDataInit
    {
        public static void SeddInfraction(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Infraction>().HasData(
                // Multas de tipo Uno
                new Infraction
                {
                    id = 1,
                    TypeInfractionId = 1,
                    description = "Irrespetar las normas propias de los lugares públicos tales como salas de velación, cementerios, clínicas, hospitales, bibliotecas y museos, entre otros.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 2,
                    TypeInfractionId = 1,
                    description = "Emplear o inducir a los niños, niñas y adolescentes a utilizar indebidamente las telecomunicaciones o sistemas de emergencia",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 3,
                    TypeInfractionId = 1,
                    description = "Utilizar a estas personas para obtener beneficio económico o satisfacer interés personal.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 4,
                    TypeInfractionId = 1,
                    description = "Limitar u obstruir las manifestaciones de afecto público que no configuren actos sexuales, de exhibicionismo en razón a la raza, orientación sexual, género u otra condición similar.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 5,
                    TypeInfractionId = 1,
                    description = "Ingresar o introducir niños, niñas o adolescentes a los actos o eventos que puedan causar daño a su integridad o en los cuales exista previa restricción de edad por parte de las autoridades de policía, o esté prohibido su ingreso por las normas vigentes.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 6,
                    TypeInfractionId = 1,
                    description = "No destruir en la fuente los envases de bebidas embriagantes.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 7,
                    TypeInfractionId = 1,
                    description = "Ingresar con boletería falsa.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 8,
                    TypeInfractionId = 1,
                    description = "Vender o canjear boletería, manillas, credencial o identificaciones facilitando el ingreso a un espectáculo público, actuando por fuera de las operaciones autorizadas para determinado evento.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 9,
                    TypeInfractionId = 1,
                    description = "Ingresar al evento sin boletería, manilla, credencial o identificación dispuesta y autorizada para el mismo o trasladarse fraudulentamente a una localidad diferente a la que acredite su boleta, manilla, credencial o identificación dispuesta y autorizada.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 10,
                    TypeInfractionId = 1,
                    description = "No informar los protocolos de seguridad y evacuación en caso de emergencias a las personas que se encuentren en el lugar.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 11,
                    TypeInfractionId = 1,
                    description = "No fijar la señalización de los protocolos de seguridad en un lugar visible.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 12,
                    TypeInfractionId = 1,
                    description = "Permitir el consumo de tabaco y/o sus derivados en lugares no autorizados por la ley y la normatividad vigente.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 13,
                    TypeInfractionId = 1,
                    description = "Comercializar, almacenar, poseer o tener especies de flora o fauna que ofrezcan peligro para la integridad y la salud.",
                    numer_smldv = 4,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },

                // Multas de tipo Dos
                new Infraction
                {
                    id = 14,
                    TypeInfractionId = 2,
                    description = "Reñir, incitar o incurrir en confrontaciones violentas que puedan derivar en agresiones físicas.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 15,
                    TypeInfractionId = 2,
                    description = "Amenazar con causar un daño físico a personas por cualquier medio.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 16,
                    TypeInfractionId = 2,
                    description = "Portar armas, elementos cortantes, punzantes o semejantes, o sustancias peligrosas, en áreas comunes o lugares abiertos al público. Se exceptúa a quien demuestre que tales elementos o sustancias constituyen una herramienta de su actividad deportiva, oficio, profesión o estudio",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 17,
                    TypeInfractionId = 2,
                    description = "Portar armas neumáticas, de aire, de fogueo, de letalidad reducida o sprays, rociadores, aspersores o aerosoles de pimienta o cualquier elemento que se asimile a armas de fuego, en lugares abiertos al público donde se desarrollen aglomeraciones de personas o en aquellos donde se consuman bebidas embriagantes, o se advierta su utilización irregular, o se incurra en un comportamiento contrario a la convivencia.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 18,
                    TypeInfractionId = 2,
                    description = "Sonidos o ruidos en actividades, fiestas, reuniones o eventos similares que afecten la convivencia del vecindario, cuando generen molestia por su impacto auditivo, en cuyo caso podrán las autoridades de Policía desactivar temporalmente la fuente del ruido, en caso de que el residente se niegue a desactivarlo;",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 19,
                    TypeInfractionId = 2,
                    description = "Destruir, averiar o deteriorar bienes dentro del área circundante de la institución o centro educativo.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 20,
                    TypeInfractionId = 2,
                    description = "Irrespetar a las autoridades de Policía.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 21,
                    TypeInfractionId = 2,
                    description = "Permitir que los niños, niñas y adolescentes sean tenedores de animales potencialmente peligrosos.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 22,
                    TypeInfractionId = 2,
                    description = "No permitir el acceso al predio sobre el cual pesa el gravamen de servidumbre para realizar el mantenimiento o la reparación.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 23,
                    TypeInfractionId = 2,
                    description = "Vender, procesar o almacenar productos alimenticios en los sitios no permitidos o contrariando las normas vigentes.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 24,
                    TypeInfractionId = 2,
                    description = "Propiciar la ocupación indebida del espacio público.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 25,
                    TypeInfractionId = 2,
                    description = "Comprar, alquilar o usar equipo terminal móvil con reporte de hurto y/o extravío en la base de datos negativa de que trata el artículo 106 de la Ley 1453 de 2011 o equipo terminal móvil cuyo número de identificación físico o electrónico haya sido reprogramado, remarcado, modificado o suprimido.",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 26,
                    TypeInfractionId = 2,
                    description = "hacer mucho ruido en un sitio publico",
                    numer_smldv = 8,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },

                // Multas de tipo Tres
                new Infraction
                {
                    id = 27,
                    TypeInfractionId = 3,
                    description = "Agredir físicamente a personas por cualquier medi",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 28,
                    TypeInfractionId = 3,
                    description = "Poner en riesgo a personas o bienes durante la instalación, utilización, mantenimiento o modificación de las estructuras de los servicios públicos.",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 29,
                    TypeInfractionId = 3,
                    description = "Modificar o alterar redes o instalaciones de servicios públicos.",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 30,
                    TypeInfractionId = 3,
                    description = "No reparar oportunamente los daños ocasionados a la infraestructura de servicios públicos domiciliarios, cuando estas reparaciones corresponden al usuario.",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 31,
                    TypeInfractionId = 3,
                    description = "Sonidos o ruidos en actividades, fiestas, reuniones o eventos similares que afecten la convivencia del vecindario, cuando generen molestia por su impacto auditivo, en cuyo caso podrán las autoridades de Policía desactivar temporalmente la fuente del ruido, en caso de que el residente se niegue a desactivarlo;",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 32,
                    TypeInfractionId = 3,
                    description = "Cualquier medio de producción de sonidos o dispositivos o accesorios o maquinaria que produzcan ruidos, desde bienes muebles o inmuebles, en cuyo caso podrán las autoridades identificar, registrar y desactivar temporalmente la fuente del ruido, salvo sean originados en construcciones o reparaciones en horas permitidas;",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 33,
                    TypeInfractionId = 3,
                    description = "Consumir bebidas alcohólicas, drogas o sustancias prohibidas, dentro de la institución o centro educativo.",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 34,
                    TypeInfractionId = 3,
                    description = "Dificultar, obstruir o limitar información e insumos relacionados con los derechos sexuales y reproductivos de la mujer, del hombre y de la comunidad LGBTI, incluido el acceso de estos a métodos anticonceptivos.",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 35,
                    TypeInfractionId = 3,
                    description = "Ejercer la prostitución sin el cumplimiento de las medidas sanitarias y de protección requeridas.",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 36,
                    TypeInfractionId = 3,
                    description = "Realizar actos sexuales o exhibicionistas en la vía pública o en lugares expuestos a esta.",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 37,
                    TypeInfractionId = 3,
                    description = "Carecer o no proporcionar los implementos de seguridad exigidos por la actividad, o proporcionarlos en mal estado de funcionamiento.",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 38,
                    TypeInfractionId = 3,
                    description = "Invadir los espacios no abiertos al público.",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 39,
                    TypeInfractionId = 3,
                    description = "Pretender ingresan o estar en posesión o tenencia de cualquier tipo de arma u objetos prohibidos por las normas vigentes, por el alcalde o su delegado",
                    numer_smldv = 16,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },

                // Multas de tipo Cuatro
                new Infraction
                {
                    id = 40,
                    TypeInfractionId = 4,
                    description = "Arrojar en las redes de alcantarillado, acueducto y de aguas lluvias, cualquier objeto, sustancia, residuo, escombros, lodo, combustibles o lubricantes, que alteren u obstruyan el normal funcionamiento.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 41,
                    TypeInfractionId = 4,
                    description = "Permitir, auspiciar, tolerar, inducir o constreñir el ingreso de los niños, niñas y adolescentes a los lugares donde:",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 42,
                    TypeInfractionId = 4,
                    description = "Salvo actos circenses, prender o manipular fuego en el espacio público, lugar abierto al público, sin contar con la autorización del alcalde o su delegado o del responsable del sitio, sin cumplir las medidas de seguridad.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 43,
                    TypeInfractionId = 4,
                    description = "Prender o manipular fuego, sustancias combustibles o mercancías peligrosas en medio de transporte público.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 44,
                    TypeInfractionId = 4,
                    description = "Fabricar, tener, portar, distribuir, transportar, comercializar, manipular o usar sustancias prohibidas, elementos o residuos químicos o inflamables sin el cumplimiento de los requisitos establecidos.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 45,
                    TypeInfractionId = 4,
                    description = "Realizar quemas o incendios que afecten la convivencia en cualquier lugar público o privado o en sitios prohibidos.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 46,
                    TypeInfractionId = 4,
                    description = "Utilizar calderas, motores, máquinas o aparatos similares que no se encuentren en condiciones aptas de funcionamiento.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 47,
                    TypeInfractionId = 4,
                    description = "Incumplir, desacatar, desconocer e impedir la función o la orden de Policía.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 48,
                    TypeInfractionId = 4,
                    description = "Impedir, dificultar, obstaculizar o resistirse a procedimiento de identificación o individualización, por parte de las autoridades de Policía.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 49,
                    TypeInfractionId = 4,
                    description = "Negarse a dar información veraz sobre lugar de residencia, domicilio y actividad a las autoridades de Policía cuando estas lo requieran en procedimientos de Policía",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 50,
                    TypeInfractionId = 4,
                    description = "Ofrecer cualquier tipo de resistencia a la aplicación de una medida o la utilización de un medio de Policía.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 51,
                    TypeInfractionId = 4,
                    description = "Agredir por cualquier medio o lanzar objetos que puedan causar daño o sustancias que representen peligro a las autoridades de Policía.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                },
                new Infraction
                {
                    id = 52,
                    TypeInfractionId = 4,
                    description = "Utilizar inadecuadamente el sistema de número único de seguridad y emergencia.",
                    numer_smldv = 32,
                    active = true,
                    is_deleted = false,
                    created_date = seedDate
                }
            );
        }
    }
}