using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.dataInitModelSecurity
{
    /// <summary>
    /// Clase estática para inicializar datos semilla (seed) para la entidad <see cref="Person"/>.
    /// </summary>
    public static class PersonDataInit
    {
        /// <summary>
        /// Método de extensión para agregar datos iniciales (seed) a la entidad <see cref="Person"/>.
        /// </summary>
        /// <param name="modelBuilder">Instancia de <see cref="ModelBuilder"/> usada para configurar el modelo de datos.</param>
        public static void seedPerson(this ModelBuilder modelBuilder)
        {

            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            // Seed para Person
            modelBuilder.Entity<Person>().HasData(
             new Person
             {
                 id = 1,
                 firstName = "Juan",
                 lastName = "Pérez",
                 municipalityId = 1,
                 active = true,
                 is_deleted = false,
                 created_date = seedDate
             },
             new Person
             {
                 id = 2,
                 firstName = "Sara",
                 lastName = "Sofía",
                 municipalityId = 4,
                 active = true,
                 is_deleted = false,
                 created_date = seedDate
             }
         );
        }
    }
}
