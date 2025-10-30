using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesParameters
{
    public class RelacionMunicipality : IEntityTypeConfiguration<municipality>
    {
        public void Configure(EntityTypeBuilder<municipality> builder)
        {
            builder.ToTable("municipality", schema: "Parameters");
            builder.ConfigureBaseModel();

            builder.HasKey(x => x.id);

            builder.Property(x => x.name)
                   .IsRequired()
                   .HasMaxLength(120)
                   .HasColumnType("varchar(120)");

            builder.Property(x => x.daneCode)
                   .IsRequired();

            builder.HasIndex(x => x.daneCode).IsUnique();
            builder.HasIndex(x => x.name);

            // municipality (N) -> department (1)
            builder.HasOne(m => m.department)
                   .WithMany(d => d.Municipality)
                   .HasForeignKey(m => m.departmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Evita nombres repetidos dentro del mismo departamento
            builder.HasIndex(m => new { m.departmentId, m.name }).IsUnique();
        }
    }
}
