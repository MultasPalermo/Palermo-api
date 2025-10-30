using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Domain.Enums
{
    public enum EstadoMulta
    {
        Pendiente,
        Pagada,
        Vencida,
        ConAcuerdoPago   // 👈 nuevo estado
    }
}
