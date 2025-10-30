using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.EntitiesDto
{
    public class InspectoraPdfDto
    {
        public string Expediente { get; set; }
        public DateTime Fecha { get; set; }
        public string InfractorNombre { get; set; }
        public string InfractorCedula { get; set; }
        public string TipoInfraccion { get; set; }
        public string DescripcionInfraccion { get; set; }
        public string Articulo { get; set; }
        public int SalariosMinimos { get; set; }
        public decimal ValorMultaPesos { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
    }
}
