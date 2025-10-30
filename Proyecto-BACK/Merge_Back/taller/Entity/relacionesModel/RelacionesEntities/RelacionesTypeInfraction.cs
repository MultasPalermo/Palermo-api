using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entity.ConfigurationsBase;

namespace Entity.relacionesModel.RelacionesEntities
{
    public class RelacionesTypeInfraction : IEntityTypeConfiguration<TypeInfraction>
    {
        public void Configure(EntityTypeBuilder<TypeInfraction> builder)
        {
            // Nombre de la tabla
            builder.ToTable("TypeInfraction", schema: "Entities");

            // Configuración de propiedades base (id, fechas, active, is_deleted, etc.)
            builder.ConfigureBaseModel();

            // Nombre de tipo de infracción obligatorio y único
            builder.Property(ti => ti.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(ti => ti.Name).IsUnique();

            // Relación uno-a-muchos: TypeInfraction -> Infractions
            builder.HasMany(ti => ti.Infractions)
                   .WithOne(i => i.TypeInfraction)
                   .HasForeignKey(i => i.TypeInfractionId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_TypeInfraction_Infraction");
        }
    }
}
