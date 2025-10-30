using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entity.ConfigurationsBase;

namespace Entity.relacionesModel.RelacionesEntities
{
    public class RelacionesValueSmldv : IEntityTypeConfiguration<ValueSmldv>
    {
        public void Configure(EntityTypeBuilder<ValueSmldv> builder)
        {
            // Nombre de tabla
            builder.ToTable("valueSmldv", schema: "Entities");

            // Propiedades del baseModel
            builder.ConfigureBaseModel();

            builder.Property(x => x.minimunWage).HasPrecision(18, 2);

            builder.Property(x => x.value_smldv) // 👈 aquí lo defines como decimal con precisión
                   .HasPrecision(18, 2);

            // Relación: ValueSmldv -> FineCalculationDetail (uno a muchos)
            builder.HasMany(vs => vs.fineCalculationDetail)
                   .WithOne(fcd => fcd.valueSmldv)
                   .HasForeignKey(fcd => fcd.valueSmldvId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_ValueSmldv_FineCalculationDetail");

            // Índice único en Current_Year
            builder.HasIndex(vs => vs.Current_Year).IsUnique();
        }
    }
}
