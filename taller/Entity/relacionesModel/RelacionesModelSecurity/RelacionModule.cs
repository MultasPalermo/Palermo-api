using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesModelSecurity
{
    public class RelacionModule : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            // Configura la tabla y el esquema
            builder.ToTable("module", schema: "ModelSecurity");

            // propiedades del baseGeneric
            builder.configureBaseModelGeneric();

            // Configura la relación uno a muchos con FormModules
            builder.HasMany(m => m.FormModules)
                   .WithOne(fm => fm.module)          // Navegación inversa en FormModule
                   .HasForeignKey(fm => fm.moduleid) // Llave foránea en FormModule
                   .OnDelete(DeleteBehavior.Restrict) // Evita eliminación en cascada
                   .HasConstraintName("FK_Module_FormModules");

            builder.HasIndex(u => u.name).IsUnique();
        }
    }
}
