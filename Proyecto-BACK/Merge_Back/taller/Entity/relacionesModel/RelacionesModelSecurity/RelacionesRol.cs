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
    public class RelacionesRol : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            // Nombre de tabla (opcional)
            builder.ToTable("rol", schema: "ModelSecurity");

            // Clave primaria
            builder.HasKey(r => r.id);

            // propiedades del baseGeneric
            builder.configureBaseModelGeneric();

            // Relación: Rol -> RolUser (uno a muchos)
            builder.HasMany(r => r.rolUsers)
                   .WithOne(ru => ru.Rol)
                   .HasForeignKey(ru => ru.RolId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Rol_RolUser");

            builder.HasIndex(u => u.name).IsUnique();
        }
    }
}
