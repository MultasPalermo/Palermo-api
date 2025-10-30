using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit
{
    public class RolUserSeeder : IEntityTypeConfiguration<RolUser>
    {
        public void Configure(EntityTypeBuilder<RolUser> builder)
        { 
            builder.HasData(
                new RolUser
                {
                    id = 1,
                    UserId = 1,
                    RolId = 1,
                    active = true,
                    is_deleted = false,
                    created_date = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
