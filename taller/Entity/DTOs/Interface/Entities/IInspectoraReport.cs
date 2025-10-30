using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Interfaces;

namespace Entity.DTOs.Interface.Entities
{
    public interface  IInspectoraReport : IHasId
    {
        int id { get; set; }
        DateTime report_date { get; set; }
        decimal total_fines { get; set; }
        string message { get; set; }
    }
}
