using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesModelSecurity
{
    public class RelacionFormModule : IEntityTypeConfiguration<FormModule>
    {
        public void Configure(EntityTypeBuilder<FormModule> builder)
        {
            // Configura la tabla y esquema
            builder.ToTable("formmodule", schema: "ModelSecurity");

            // Clave primaria
            builder.HasKey(fm => fm.id);

            // Propiedades básicas
            builder.ConfigureBaseModel();

            //propiedad de form id
            builder.Property(fm => fm.formid).HasColumnName("formid");

            //propiedad de module id
            builder.Property(fm => fm.moduleid).HasColumnName("moduleid");

            // Propiedad requerida
            builder.Property(fm => fm.is_deleted)
                   .IsRequired();

            // Relación muchos a uno con Form
            builder.HasOne(fm => fm.form)
                   .WithMany(f => f.FormModules)
                   .HasForeignKey(fm => fm.formid)
                   .OnDelete(DeleteBehavior.Restrict)  // Evita borrado en cascada
                   .HasConstraintName("FK_FormModule_Form");

            // Relación muchos a uno con Module
            builder.HasOne(fm => fm.module)
                   .WithMany(m => m.FormModules)
                   .HasForeignKey(fm => fm.moduleid)
                   .OnDelete(DeleteBehavior.Restrict)  // Evita borrado en cascada
                   .HasConstraintName("FK_FormModule_Module");
        }
    }
}
