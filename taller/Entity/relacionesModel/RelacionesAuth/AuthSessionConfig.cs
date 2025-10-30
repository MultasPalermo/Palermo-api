// Entity.Infrastructure/Configurations/ModelSecurity/AuthSessionConfig.cs
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AuthSessionConfig : IEntityTypeConfiguration<AuthSession>
{
    public void Configure(EntityTypeBuilder<AuthSession> b)
    {
        b.ToTable("AuthSession", "ModelSecurity");   // usa tu esquema si corresponde
        b.HasKey(x => x.id);                         // si tu BaseModel tiene Id PK
        b.HasIndex(x => x.SessionId).IsUnique();


        b.Property(x => x.SessionId).IsRequired();
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.LastActivityAt).IsRequired();
        b.Property(x => x.AbsoluteExpiresAt).IsRequired();
        b.Property(x => x.IsRevoked).IsRequired();

        // campos opcionales:
        b.Property(x => x.Ip).HasMaxLength(64);
        b.Property(x => x.UserAgent).HasMaxLength(512);
    }
}
