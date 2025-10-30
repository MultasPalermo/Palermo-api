using Microsoft.EntityFrameworkCore;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace Entity.DataInit.dataInitModelSecurity
{
    /// <summary>
    /// Clase estática para inicializar datos semilla para la entidad RolFormPermission.
    /// </summary>
    public static class RolFormPermissionDataInit
    {
        /// <summary>
        /// Método de extensión para agregar datos iniciales (seed) para RolFormPermission.
        /// </summary>
        /// <param name="modelBuilder">El constructor del modelo para configuración.</param>
        public static void SeedRolFormPermission(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            var rolFormPermissions = new List<RolFormPermission>();
            int id = 1;

            // ===========================
            // Permisos para Administrador
            // ===========================
            for (int formId = 1; formId <= 21; formId++) // todos los formularios
            {
                for (int permissionId = 1; permissionId <= 6; permissionId++) // todos los permisos
                {
                    rolFormPermissions.Add(new RolFormPermission
                    {
                        id = id++,
                        RolId = 1, // Administrador
                        FormId = formId,
                        PermissionId = permissionId,
                        is_deleted = false,
                        created_date = seedDate
                    });
                }
            }

            // ===========================
            // Permisos para Finanzas
            // Solo leer Notificación de multas (4) y Perfil (18)
            // ===========================
            rolFormPermissions.Add(new RolFormPermission
            {
                id = id++,
                RolId = 2, 
                FormId = 4, 
                PermissionId = 1,
                is_deleted = false,
                created_date = seedDate
            });

            rolFormPermissions.Add(new RolFormPermission
            {
                id = id++,
                RolId = 2, 
                FormId = 18,
                PermissionId = 1, 
                is_deleted = false,
                created_date = seedDate
            });         
            rolFormPermissions.Add(new RolFormPermission
            {
                id = id++,
                RolId = 2, 
                FormId = 19,
                PermissionId = 1, 
                is_deleted = false,
                created_date = seedDate
            });
            rolFormPermissions.Add(new RolFormPermission
            {
                id = id++,
                RolId = 2,
                FormId = 20,
                PermissionId = 1,
                is_deleted = false,
                created_date = seedDate
            }
            );


            modelBuilder.Entity<RolFormPermission>().HasData(rolFormPermissions);
        }
    }
}
