using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entity.Domain.Models.Implements.ModelSecurity;
using Entity.ConfigurationsBase;

namespace Entity.relacionesModel.RelacionesModelSecurity
{
    public class RelacionRolUser : IEntityTypeConfiguration<RolUser>
    {
        public void Configure(EntityTypeBuilder<RolUser> builder)
        {
            // Definición de tabla y esquema
            builder.ToTable("roluser", schema: "ModelSecurity");

            //propiedad de baseModel
            builder.ConfigureBaseModel();

            //propiedad de user id
            builder.Property(u => u.UserId).HasColumnName("userId");

            //propiedad de rol id 
            builder.Property(r => r.RolId).HasColumnName("rolId");

            // Clave primaria
            builder.HasKey(ru => ru.id);

            // Relación muchos a uno con Rol
            builder.HasOne(ru => ru.Rol)
                   .WithMany(r => r.rolUsers)  
                   .HasForeignKey(ru => ru.RolId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_RolUser_Rol");

            // Relación muchos a uno con User
            builder.HasOne(ru => ru.User)
                   .WithMany(u => u.rolUsers)  
                   .HasForeignKey(ru => ru.UserId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_RolUser_User");
        }
    }
}
