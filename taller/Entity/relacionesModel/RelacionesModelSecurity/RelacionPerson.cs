// Entity.relacionesModel.RelacionesModelSecurity.RelacionPerson
using Entity.ConfigurationsBase;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesModelSecurity
{
    public class RelacionPerson : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("person", schema: "ModelSecurity");
            builder.ConfigureBaseModel();

            builder.HasKey(p => p.id);

            // Obligatorios
            builder.Property(p => p.firstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.lastName)
                   .IsRequired()
                   .HasMaxLength(100);
   
            // FK opcional: municipality
            builder.HasOne(p => p.municipality)
                   .WithMany(m => m.person)
                   .HasForeignKey(p => p.municipalityId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
