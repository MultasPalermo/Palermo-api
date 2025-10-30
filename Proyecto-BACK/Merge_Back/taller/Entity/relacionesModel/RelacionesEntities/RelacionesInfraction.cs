using System;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entity.ConfigurationsBase;

namespace Entity.relacionesModel.RelacionesEntities
{
    // 2. TypeInfraction Configuration
    public class RelacionesInfraction : IEntityTypeConfiguration<Infraction>
    {
        public void Configure(EntityTypeBuilder<Infraction> builder)
        {
            // Nombre de tabla
            builder.ToTable("Infraction", schema: "Entities");

            // Configuración de propiedades base
            builder.ConfigureBaseModel();

            // Campos propios
            builder.Property(i => i.description)
                   .HasMaxLength(250);

            builder.Property(i => i.numer_smldv)
                   .IsRequired();


            // Relación: TypeInfraction -> UserInfraction (uno a muchos)
            //builder.HasMany(ti => ti.userInfractions)
            //       .WithOne(ui => ui.typeInfraction)
            //       .HasForeignKey(ui => ui.typeInfractionId)
            //       .OnDelete(DeleteBehavior.Restrict)
            //       .HasConstraintName("FK_TypeInfraction_UserInfraction");

            // Relación: TypeInfraction -> FineCalculationDetail (uno a muchos)
            builder.HasMany(ti => ti.fineCalculationDetail)
                   .WithOne(fcd => fcd.Infraction)
                   .HasForeignKey(fcd => fcd.typeInfractionId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_TypeInfraction_FineCalculationDetail");

        }
    }
}
