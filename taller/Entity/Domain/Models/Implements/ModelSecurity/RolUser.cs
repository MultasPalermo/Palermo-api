using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.ModelSecurity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RolUser : BaseModel
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int RolId { get; set; }

    // Relaciones de navegación
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(RolId))]
    public Rol Rol { get; set; }
}
