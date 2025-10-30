using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesParameters
{
    public class RelacionDocumentType : IEntityTypeConfiguration<documentType>
    {
        public void Configure(EntityTypeBuilder<documentType> builder)
        {
            builder.ToTable("documentType", schema: "Parameters");
            builder.ConfigureBaseModel();

            builder.HasKey(x => x.id);

            builder.Property(x => x.name)
                   .IsRequired()
                   .HasMaxLength(80)
                   .HasColumnType("varchar(80)");

            builder.Property(x => x.abbreviation)
                   .IsRequired()
                   .HasMaxLength(10)
                   .HasColumnType("varchar(10)");

            builder.HasIndex(x => x.abbreviation).IsUnique();
            builder.HasIndex(x => x.name);
        }
    }
}
