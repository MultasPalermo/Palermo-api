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
    /// Clase estática para inicializar datos de la entidad <see cref="FormModule"/>.
    /// </summary>
    public static class FormModuleDataInit
    {
        /// <summary>
        /// Método de extensión para inicializar datos semilla (seed) para la entidad <see cref="FormModule"/>.
        /// </summary>
        /// <param name="modelBuilder">Instancia de <see cref="ModelBuilder"/> usada para configurar el modelo de datos.</param>
        public static void SeedFormModule(this ModelBuilder modelBuilder)
        {

            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<FormModule>().HasData(
                 //new FormModule
                 //{
                 //    id = 1,
                 //    formid = 1,    
                 //    moduleid = 2, 
                 //    is_deleted = false,
                 //    created_date = seedDate,
                 //},
                 new FormModule
                 {
                     id = 1,
                     formid = 2,
                     moduleid = 2,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 //new FormModule
                 //{
                 //    id = 2,
                 //    formid = 3,
                 //    moduleid = 2,
                 //    is_deleted = false,
                 //    created_date = seedDate,
                 //},
                 new FormModule
                 {
                     id = 2,
                     formid = 4,
                     moduleid = 2,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 3,
                     formid = 5,
                     moduleid = 3,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 4,
                     formid = 6,
                     moduleid = 3,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 5,
                     formid = 7,
                     moduleid = 3,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 6,
                     formid = 8,
                     moduleid = 3,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 7,
                     formid = 9,
                     moduleid = 3,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 8,
                     formid = 10,
                     moduleid = 3,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 9,
                     formid = 11,
                     moduleid = 3,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 //new FormModule
                 //{
                 //    id = 10,
                 //    formid = 12,
                 //    moduleid = 3,
                 //    is_deleted = false,
                 //    created_date = seedDate,
                 //},
                 new FormModule
                 {
                     id = 10,
                     formid = 13,
                     moduleid = 3,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 11,
                     formid = 14,
                     moduleid = 5,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 12,
                     formid = 15,
                     moduleid = 5,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 13,
                     formid = 16,
                     moduleid = 5,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 14,
                     formid = 17,
                     moduleid = 5,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 15,
                     formid = 18,
                     moduleid = 2,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 16,
                     formid = 19,
                     moduleid = 2,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 17,
                     formid = 20,
                     moduleid = 2,
                     is_deleted = false,
                     created_date = seedDate,
                 },
                 new FormModule
                 {
                     id = 18,
                     formid = 21,
                     moduleid = 5,
                     is_deleted = false,
                     created_date = seedDate,
                 }
             );
        }
    }
}
