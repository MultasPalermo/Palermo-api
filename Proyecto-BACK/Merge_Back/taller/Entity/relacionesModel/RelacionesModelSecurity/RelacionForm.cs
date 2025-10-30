using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesModelSecurity
{
    public class RelacionForm : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            // Tabla (opcional)
            builder.ToTable("form", schema: "ModelSecurity");

            // Clave primaria
            builder.HasKey(f => f.id);

            // propiedades del baseGeneric
            builder.configureBaseModelGeneric();

            // Relación: Form -> FormModules (uno a muchos)
            builder.HasMany(f => f.FormModules)
                   .WithOne(fm => fm.form) 
                   .HasForeignKey(fm => fm.formid)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Form_FormModules");

            // Relación: Form -> RolFormPermission (uno a muchos)
            builder.HasMany(f => f.rol_form_permission)
                   .WithOne(rfp => rfp.Form) 
                   .HasForeignKey(rfp => rfp.FormId) 
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Form_RolFormPermission");

            builder.HasIndex(u => u.name).IsUnique();
        }
    }
}
