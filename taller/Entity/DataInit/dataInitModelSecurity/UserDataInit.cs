// Entity.DataInit.dataInitModelSecurity.UserDataInit
using System;
using Microsoft.EntityFrameworkCore;
using Entity.Domain.Models.Implements.ModelSecurity;

namespace Entity.DataInit.dataInitModelSecurity
{
    public static class UserDataInit
    {
        public static void SeedUser(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<User>().HasData(
                 new User
                 {
                     id = 1,
                     PasswordHash = "admin123",
                     email = "camiloandreslosada901@gmail.com",
                     active = true,
                     is_deleted = false,
                     PersonId = 1,
                     documentTypeId = 1,
                     documentNumber = "1234567890",
                     created_date = seedDate,

                     // Nuevos campos de verificación
                     EmailVerified = true,
                     EmailVerifiedAt = seedDate,
                     EmailVerificationCode = null,
                     EmailVerificationExpiresAt = null,
                   
                 },
                 new User
                 {
                     id = 2,
                     PasswordHash = "sara12312",
                     email = "sarita@gmail.com",
                     active = true,
                     is_deleted = false,
                     PersonId = 2,
                     documentTypeId = 2,
                     documentNumber = "0123432121",
                     created_date = seedDate,

                     // Nuevos campos de verificación
                     EmailVerified = true,
                     EmailVerifiedAt = seedDate,
                     EmailVerificationCode = null,
                     EmailVerificationExpiresAt = null,
                  
                 }

                    
            );
        }
    }
}
