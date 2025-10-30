using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesParameters
{
    public class RelacionDepartment : IEntityTypeConfiguration<department>
    {
        public void Configure(EntityTypeBuilder<department> builder)
        {
            builder.ToTable("department", schema: "Parameters");
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

            // department (1) -> municipality (N)
            builder.HasMany(d => d.Municipality)
               .WithOne(m => m.department)
               .HasForeignKey(m => m.departmentId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
