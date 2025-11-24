    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Entity.Domain.Enums
    {
    public enum EstadoCobro
    {
        CobroPrejuridico = 0, 
        prejuridico3Dias = 1, 
        prejuridico15Dias = 2, 
        prejuridico25Dias = 3,
        CobroJuridico = 4,
        CobroCoactivo = 5
    }
}
