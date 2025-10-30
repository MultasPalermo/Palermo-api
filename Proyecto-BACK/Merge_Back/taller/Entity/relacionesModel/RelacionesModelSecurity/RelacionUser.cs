// Entity.relacionesModel.RelacionesModelSecurity.RelacionUser
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.relacionesModel.RelacionesModelSecurity
{
    public class RelacionUser : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user", schema: "ModelSecurity");

            // ----- Campos básicos -----

            builder.Property(p => p.PasswordHash)
                   .HasMaxLength(100)
                   .IsUnicode(false);

            // Email opcional (nullable en BD)
            builder.Property(e => e.email)
                   .IsRequired(false)          
                   .HasMaxLength(150)
                   .IsUnicode(false);

            // ----- Campos de verificación de email -----

            builder.Property(u => u.EmailVerified)
                   .HasDefaultValue(false);    

            builder.Property(u => u.EmailVerificationCode)
                   .HasMaxLength(6)
                   .IsUnicode(false);

            builder.Property(u => u.EmailVerificationExpiresAt);
            builder.Property(u => u.EmailVerifiedAt);

            // ----- Relaciones -----

            // 1:1 opcional User <-> Person (sin cascada)
            builder.HasOne(u => u.Person)
                   .WithOne(p => p.User)
                   .HasForeignKey<User>(u => u.PersonId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_User_Person");

            // FK opcional: documentType
            builder.HasOne(p => p.documentType)
                   .WithMany(dt => dt.person)
                   .HasForeignKey(p => p.documentTypeId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
