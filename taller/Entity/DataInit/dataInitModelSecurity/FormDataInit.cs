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
    /// Clase estática para inicializar datos de la entidad <see cref="Form"/>.
    /// </summary>
    public static class FormDataInit
    {
        /// <summary>
        /// Método de extensión para inicializar datos semilla (seed) para la entidad <see cref="Form"/>.
        /// </summary>
        /// <param name="modelBuilder">Instancia de <see cref="ModelBuilder"/> usada para configurar el modelo de datos.</param>
        public static void SeedForm(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Form>().HasData(
                new Form
                {
                    id = 1,
                    name = "Formulario de acuerdo de pago",
                    description = "Formulario de creacion de acuerdo de pago",
                    active = true,
                    Route = "acuerdoPago",
                    Icon = "pi pi-fw pi-home",
                    is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 2,
                    name = "Formulario de creacion de multas",
                    description = "Formulario para agregar nuevas multas",
                    active = true,
                    Route = "anexar-multas/multas",
                    Icon = "pi pi-fw pi-homeing",
                    is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 3,
                    name = "Formulario tipo de  multas",
                    description = "Formulario tipo de  multas",
                    active = true,
                    Route = "tipos-multas",
                    Icon = "pi pi-fw pi-id-card",
                    is_deleted = false,
                    created_date = seedDate,
                },
                 new Form
                {
                    id = 4,
                    name = "Notificacion de multas",
                    description = "Formulario Notificacion de multas",
                    active = true,
                     Route = "notificaciones/notificacion-multas",
                     Icon = "pi pi-fw pi-check-square",
                     is_deleted = false,
                    created_date = seedDate,
                },
               new Form
                {
                    id = 5,
                    name = "Formularios",
                    description = "Formularios",
                    active = true,
                     Route = "formularios",
                   Icon = "pi pi-fw pi-file",
                   is_deleted = false,
                    created_date = seedDate,
                },
                 new Form
                {
                    id = 6,
                    name = "Formularios y  modules",
                    description = "Formularios y  modules",
                    active = true,
                     Route = "form-modules",
                     Icon = "pi pi-fw pi-clone",
                     is_deleted = false,
                    created_date = seedDate,
                },
                  new Form
                  {
                      id = 7,
                      name = "Modulos",
                      description = "Modulos",
                      active = true,
                      Route = "modulos",
                      Icon = "pi pi-fw pi-th-large",
                      is_deleted = false,
                      created_date = seedDate,
                  },
                  new Form
                {
                    id = 8,
                    name = "personas",
                    description = "Personas",
                    active = true,
                     Route = "personas",
                      Icon = "pi pi-fw pi-users",
                      is_deleted = false,
                    created_date = seedDate,
                },
               new Form
                {
                    id = 9,
                    name = "permisos",
                    description = "permisos",
                    active = true,
                     Route = "permisos",
                   Icon = "pi pi-fw pi-lock-open",
                   is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 10,
                    name = "Roles Formularios y Permission",
                    description = "Roles Formularios y Permission",
                    active = true,
                     Route = "rol-form-permission",
                    Icon = "pi pi-fw pi-key",
                    is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 11,
                    name = "Roles",
                    description = "Roles",
                    active = true,
                     Route = "roles",
                    Icon = "pi pi-fw pi-users",
                    is_deleted = false,
                    created_date = seedDate,
                },
               new Form
                {
                    id = 12,
                    name = "Usuarios",
                    description = "Usuarios",
                    active = true,
                     Route = "usuarios",
                   Icon = "pi pi-fw pi-user",
                   is_deleted = false,
                    created_date = seedDate,
                },
                 new Form
                {
                    id = 13,
                    name = "Roles y Usuario",
                    description = "Roles y Usuario",
                    active = true,
                     Route = "rol-user",
                     Icon = "pi pi-fw pi-user-plus",
                     is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 14,
                    name = "departamento",
                    description = "departamento",
                    active = true,
                     Route = "parameters/department",
                    Icon = "pi pi-fw pi-briefcase",
                    is_deleted = false,
                    created_date = seedDate,
                },
                  new Form
                {
                    id = 15,
                    name = "Tipo de Documento",
                    description = "Tipo de Documento",
                    active = true,
                     Route = "parameters/document-type",
                      Icon = "pi pi-fw pi-briefcase",
                      is_deleted = false,
                    created_date = seedDate,
                },
                 new Form
                {
                    id = 16,
                    name = "Municipio",
                    description = "Municipio",
                    active = true,
                     Route = "parameters/municipality",
                     Icon = "pi pi-fw pi-briefcase",
                     is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 17,
                    name = "Frecuencia de pago ",
                    description = "Frecuencia de pago",
                    active = true,
                     Route = "parameters/payment-frequency",
                    Icon = "pi pi-fw pi-briefcase",
                    is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 18,
                    name = "Perfil",
                    description = "Perfil",
                    active = true,
                     Route = "dashboard",
                    Icon = "pi pi-fw pi-briefcase",
                    is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 19,
                    name = "Notificacion de acuerdo",
                    description = "Notificacion de acuerdo ",
                    active = true,
                     Route = "notificaciones",
                    Icon = "pi pi-fw pi-briefcase",
                    is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 20,
                    name = "inicio",
                    description = "inicio ",
                    active = true,
                     Route = "consultar-ingresar",
                     Icon = "pi pi-fw pi-home",
                     is_deleted = false,
                    created_date = seedDate,
                },
                new Form
                {
                    id = 21,
                    name = "valor de SMDLV",
                    description = "valor de SMDLV ",
                    active = true,
                    Route = "parameters/smdlv",
                    Icon = "pi pi-fw pi-home",
                    is_deleted = false,
                    created_date = seedDate,
                }

            );
        }
    }
}
