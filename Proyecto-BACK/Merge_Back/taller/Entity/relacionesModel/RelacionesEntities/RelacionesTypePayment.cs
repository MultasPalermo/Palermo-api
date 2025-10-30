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
    public class RelacionesTypePayment : IEntityTypeConfiguration<TypePayment>
    {
        public void Configure(EntityTypeBuilder<TypePayment> builder)
        {
            builder.ToTable("typePayment", schema: "Entities");

            builder.ConfigureBaseModel();

            builder.HasIndex(tp => tp.name).IsUnique();

            // relación 1:N desde TypePayment -> PaymentAgreement
            builder.HasMany(tp => tp.PaymentAgreements)
                   .WithOne(pa => pa.TypePayment)
                   .HasForeignKey(pa => pa.typePaymentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
