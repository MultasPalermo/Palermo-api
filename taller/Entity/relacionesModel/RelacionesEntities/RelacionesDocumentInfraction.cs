using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesEntities
{
    public class RelacionesDocumentInfraction : IEntityTypeConfiguration<DocumentInfraction>
    {
        public void Configure(EntityTypeBuilder<DocumentInfraction> builder)
        {
            // Configura la tabla y esquema
            builder.ToTable("DocumentInfraction", schema: "Entities");

            // Propiedades del baseModel
            builder.ConfigureBaseModel();

            // Propiedades de clave foránea
            builder.Property(ins => ins.inspectoraReportId).HasColumnName("inspectoraReportId");
            builder.Property(pay => pay.PaymentAgreementId).HasColumnName("PaymentAgreementId");

            // Relaciones
            builder.HasOne(di => di.paymentAgreement)
                   .WithMany(pa => pa.documentInfraction)
                   .HasForeignKey(di => di.PaymentAgreementId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_DocumentInfraction_PaymentAgreement");

            builder.HasOne(di => di.inspectoraReport)
                   .WithMany(ir => ir.documentInfraction)
                   .HasForeignKey(di => di.inspectoraReportId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_DocumentInfraction_InspectoraReport");

            // Propiedad requerida
            builder.Property(i => i.is_deleted)
                   .IsRequired();

        }
    }
}
