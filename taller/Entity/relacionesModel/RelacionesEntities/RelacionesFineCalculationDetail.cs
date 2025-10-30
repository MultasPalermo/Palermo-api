using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entity.Domain.Models.Implements.Entities;
using Entity.ConfigurationsBase;

public class RelacionesFineCalculationDetail : IEntityTypeConfiguration<FineCalculationDetail>
{
    public void Configure(EntityTypeBuilder<FineCalculationDetail> builder)
    {
        // Nombre de tabla
        builder.ToTable("FineCalculationDetail", "Entities");

        // Propiedades base (id, fechas, active, is_deleted, etc.)
        builder.ConfigureBaseModel();

        // Propiedad obligatoria: fórmula usada en el cálculo
        builder.Property(x => x.formula)
               .HasMaxLength(200)
               .IsRequired();

        // Total del cálculo (resultado final en pesos)
        builder.Property(x => x.totalCalculation)
               .HasColumnType("decimal(12,2)")
               .IsRequired();

        // Relación: FineCalculationDetail -> TypeInfraction (muchos a uno)
        builder.HasOne(x => x.Infraction)
               .WithMany(t => t.fineCalculationDetail)
               .HasForeignKey(x => x.typeInfractionId)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_TypeInfraction_FineCalculationDetail");

        // Relación: FineCalculationDetail -> ValueSmldv (muchos a uno)
        builder.HasOne(x => x.valueSmldv)
               .WithMany(v => v.fineCalculationDetail)
               .HasForeignKey(x => x.valueSmldvId)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_ValueSmldv_FineCalculationDetail");

        // 🔒 Evita que EF cree columnas sombra por convenciones
        builder.Ignore("TypeInfractionid");
        builder.Ignore("ValueSmldvid");
    }
}
