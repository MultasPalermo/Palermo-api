using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;
using Entity.DTOs.Interface.Entities;

namespace Entity.Domain.Models.Implements.Entities
{
    public class InspectoraReportSelectDto : IInspectoraReport
    {
        public int id { get; set; }
        public DateTime report_date { get; set; } 
        public decimal total_fines { get; set; }
        public string message { get; set; }
    }
}
