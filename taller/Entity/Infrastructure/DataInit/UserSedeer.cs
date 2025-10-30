using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    public class UserSedeer : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                                 new User
                                 {
                                     id = 1,
                                     //name = "admin",
                                     email = "admin@example.com",
                                     PasswordHash = "admin123",
                                     active = true,
                                     is_deleted = false,
                                     PersonId = null, 
                                     created_date = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                                 }
                            );

        }
    }
}
