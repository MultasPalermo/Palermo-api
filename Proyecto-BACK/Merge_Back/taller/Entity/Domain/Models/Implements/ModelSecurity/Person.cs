// Entity.Domain.Models.Implements.ModelSecurity.Person
using Entity.Domain.Enums;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.parameters;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Domain.Models.Implements.ModelSecurity
{
    public class Person : BaseModel
    {
        [Column(TypeName = "varchar(100)")]
        public string firstName { get; set; } = null!;

        [Column(TypeName = "varchar(100)")]
        public string lastName { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? phoneNumber { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? address { get; set; }
        public TipoUsuario tipoUsuario { get; set; } = TipoUsuario.PersonaNormal;

        public int? municipalityId { get; set; }
        public municipality? municipality { get; set; }

        // 1:1 opcional con User
        public User? User { get; set; }
    }
}
