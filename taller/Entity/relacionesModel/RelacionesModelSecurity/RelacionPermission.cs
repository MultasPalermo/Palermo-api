using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.ConfigurationsBase;

namespace Entity.relacionesModel.RelacionesModelSecurity
{
    public class RelacionPermission : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            // Configura la tabla y el esquema
            builder.ToTable("permission", schema: "ModelSecurity");

            // propiedades del baseGeneric
            builder.configureBaseModelGeneric();

            // Relación uno a muchos con RolFormPermission
            builder.HasMany(p => p.rol_form_permission)
                   .WithOne(rfp => rfp.Permission)
                   .HasForeignKey(rfp => rfp.PermissionId)
                   .OnDelete(DeleteBehavior.Restrict) // Evita borrado en cascada
                   .HasConstraintName("FK_Permission_RolFormPermission");

            builder.HasIndex(u => u.name).IsUnique();
        }
    }
}
