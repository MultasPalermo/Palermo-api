using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.relacionesModel.RelacionesEntities
{
    public class RelacionesInstallmentSchedule : IEntityTypeConfiguration<InstallmentSchedule>
    {
        public void Configure(EntityTypeBuilder<InstallmentSchedule> builder)
        {
            builder.ToTable("installmentSchedule", schema: "Entities");

            builder.ConfigureBaseModel();

            builder.Property(i => i.Number)
                   .IsRequired();

            builder.Property(i => i.PaymentDate)
                   .IsRequired();

            builder.Property(i => i.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.RemainingBalance)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.IsPaid)
                   .HasDefaultValue(false);

            // 🔗 Relación con PaymentAgreement (uno a muchos)
            builder.HasOne(i => i.PaymentAgreement)
                   .WithMany(p => p.InstallmentSchedule)
                   .HasForeignKey(i => i.PaymentAgreementId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_InstallmentSchedule_PaymentAgreement");
        }
    }
}
