using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default.AnexarMulta
{
   public class CreateInfractionRequestDto
    {
        // Persona
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }

        // Multa
        public int TypeInfractionId { get; set; }
        public int SmldvCount { get; set; }
    }
}
