// Entity.Domain.Models.Implements.ModelSecurity.User
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.parameters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
    public class User : BaseModel
    {
        [Column(TypeName = "varchar(100)")]
        public string? PasswordHash { get; set; } = null!;

        [Column(TypeName = "varchar(150)")]
        public string? email { get; set; }   // opcional si se usa documento

        // Foreign key
        [ForeignKey("Person")]
        public int? PersonId { get; set; }
        public Person? Person { get; set; }

        public int? documentTypeId { get; set; }
        public documentType? documentType { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? documentNumber { get; set; }

        public bool EmailVerified { get; set; } = false;
        public string? EmailVerificationCode { get; set; }
        public DateTime? EmailVerificationExpiresAt { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }

        public List<UserInfraction> UserInfraction { get; set; } = new();
        public List<RolUser> rolUsers { get; set; } = new();
    }


}
