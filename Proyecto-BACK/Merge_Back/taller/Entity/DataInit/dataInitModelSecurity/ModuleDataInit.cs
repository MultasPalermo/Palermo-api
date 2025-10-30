using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;

namespace Entity.DataInit.dataInitModelSecurity
{
    /// <summary>
    /// Clase estática para inicializar datos semilla (seed) de la entidad <see cref="Module"/>.
    /// </summary>
    public static class ModuleDataInit
    {
        /// <summary>
        /// Método de extensión para agregar datos iniciales (seed) a la entidad <see cref="Module"/>.
        /// </summary>
        /// <param name="modelBuilder">Instancia de <see cref="ModelBuilder"/> usada para configurar el modelo de datos.</param>
        public static void SeedModule(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Module>().HasData(
                new Module
                {
                    id = 1,
                    name = "Inicio",
                    description = "Inicio",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                },
                new Module
                {
                    id = 2,
                    name = "Contenido",
                    description = "Contenido",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                },
                new Module
                {
                    id = 3 ,
                    name = "Gestion Avanzada",
                    description = "Gestion Avanzada",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                },
                 new Module
                {
                    id = 4 ,
                    name = "perfil",
                    description = "perfil",
                    active = true,
                    is_deleted = false,
                    created_date = seedDate,
                },
                 new Module
                 {
                     id = 5,
                     name = "modulo de parametro",
                     description = "modulo de parametro",
                     active = true,
                     is_deleted = false,
                     created_date = seedDate,
                 }
            );
        }
    }
}
