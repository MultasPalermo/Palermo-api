using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.parameters;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesParameters
{
    public class RelacionPaymentFrequency : IEntityTypeConfiguration<PaymentFrequency>
    {
        public void Configure(EntityTypeBuilder<PaymentFrequency> builder)
        {
            builder.ToTable("paymentFrequency", schema: "Parameters");
            builder.ConfigureBaseModel();

            builder.HasKey(x => x.id);

            builder.Property(x => x.intervalPage)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnType("varchar(50)");

            builder.Property(x => x.dueDayOfMonth)
                   .IsRequired();


            builder.HasIndex(x => x.intervalPage).IsUnique(false);
        }
    }
}
