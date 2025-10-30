using Entity.Domain.Models.Implements.Entities;
using Entity.ConfigurationsBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesEntities
{
    public class RelacionesPaymentAgreement : IEntityTypeConfiguration<PaymentAgreement>
    {
        public void Configure(EntityTypeBuilder<PaymentAgreement> builder)
        {
            builder.ToTable("paymentAgreement", schema: "Entities");
            builder.ConfigureBaseModel();

            builder.Property(p => p.address)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(p => p.neighborhood)
                   .HasMaxLength(150)
                   .IsRequired(false);

            builder.Property(p => p.AgreementDescription)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(p => p.expeditionCedula)
                   .IsRequired(false); // ✅ ahora opcional

            builder.Property(p => p.PhoneNumber)
                   .HasMaxLength(20)
                   .IsRequired(false);

            builder.Property(p => p.Email)
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.Property(p => p.AgreementStart).IsRequired();
            builder.Property(p => p.AgreementEnd).IsRequired();

            builder.Property(p => p.BaseAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.AccruedInterest)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0);

            builder.Property(p => p.OutstandingAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.MonthlyFee)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired(false);

            builder.Property(p => p.IsPaid).HasDefaultValue(false);
            builder.Property(p => p.IsCoactive).HasDefaultValue(false);

            // 🔹 Relaciones
            builder.HasOne(pa => pa.userInfraction)
                   .WithMany(ui => ui.paymentAgreement)
                   .HasForeignKey(pa => pa.userInfractionId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_PaymentAgreement_UserInfraction");

            builder.HasOne(pa => pa.paymentFrequency)
                   .WithMany(pf => pf.paymentAgreement)
                   .HasForeignKey(pa => pa.paymentFrequencyId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(pa => pa.TypePayment)
                   .WithMany(tp => tp.PaymentAgreements)
                   .HasForeignKey(pa => pa.typePaymentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_PaymentAgreement_TypePayment");

            // 🔹 Relación con InstallmentSchedule
            builder.HasMany(pa => pa.InstallmentSchedule)
                   .WithOne(isched => isched.PaymentAgreement)
                   .HasForeignKey(isched => isched.PaymentAgreementId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
