using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    public class RolSeeder : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.HasData(
                new Rol
                {
                    id = 1,
                    name = "Administrador",
                    description = "Rol con permisos administrativos",
                    active = true,
                    is_deleted = false,
                    created_date = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                },
                new Rol
                {
                    id = 2,
                    name = "Usuario",
                    description = "Rol con permisos de usuario",
                    active = true,
                    is_deleted = false,
                    created_date = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)

                }
            );
        }
    }
}
