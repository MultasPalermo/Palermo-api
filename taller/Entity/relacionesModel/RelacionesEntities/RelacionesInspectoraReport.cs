using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesEntities
{
    public class RelacionesInspectoraReport : IEntityTypeConfiguration<InspectoraReport>
    {
        public void Configure(EntityTypeBuilder<InspectoraReport> builder)
        {
            // Tabla y esquema
            builder.ToTable("InspectoraReport", schema: "Entities");

            // BaseModel (id, active, is_deleted, created_date, etc.)
            builder.ConfigureBaseModel();

            builder.Property(x => x.report_date)
                   .IsRequired();


            // Decimal con precisión
            builder.Property(x => x.total_fines)
                   .HasPrecision(18, 2);
        }
    }
}
