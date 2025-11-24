using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.@interface
{
    public interface IInfractionDiscountRunner
    {
        // Este es el método que se llamará inmediatamente al crear la multa.
        Task RunOnceFor(int infractionId);
    }
}
