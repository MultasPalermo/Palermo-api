using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.ConfigurationsBase;

namespace Entity.relacionesModel.RelacionesModelSecurity
{
    public class RelacionRolFormPermission : IEntityTypeConfiguration<RolFormPermission>
    {
        public void Configure(EntityTypeBuilder<RolFormPermission> builder)
        {
            // Tabla y esquema
            builder.ToTable("rolformpermission", schema: "ModelSecurity");

            // propiedad de BaseModel
            builder.ConfigureBaseModel();

            // Configuración de propiedades escalares (NO propiedades de navegación)
            builder.Property(rfp => rfp.RolId)
                   .HasColumnName("rolid")
                   .IsRequired();

            builder.Property(rfp => rfp.FormId)
                   .HasColumnName("formid")
                   .IsRequired();

            builder.Property(rfp => rfp.PermissionId)
                   .HasColumnName("permissionid")
                   .IsRequired();

            // Relación muchos a uno con Rol
            builder.HasOne(rfp => rfp.Rol)
                   .WithMany(r => r.rol_form_permission)
                   .HasForeignKey(rfp => rfp.RolId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_RolFormPermission_Rol");

            // Relación muchos a uno con Form
            builder.HasOne(rfp => rfp.Form)
                   .WithMany(f => f.rol_form_permission)
                   .HasForeignKey(rfp => rfp.FormId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_RolFormPermission_Form");

            // Relación muchos a uno con Permission
            builder.HasOne(rfp => rfp.Permission)
                   .WithMany(p => p.rol_form_permission)
                   .HasForeignKey(rfp => rfp.PermissionId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_RolFormPermission_Permission");

            // Índice compuesto para evitar duplicados
            builder.HasIndex(rfp => new { rfp.RolId, rfp.FormId, rfp.PermissionId })
                   .IsUnique()
                   .HasDatabaseName("IX_RolFormPermission_Unique");
        }
    }
}