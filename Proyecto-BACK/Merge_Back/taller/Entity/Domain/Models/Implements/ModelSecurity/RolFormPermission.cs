using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.ModelSecurity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class RolFormPermission : BaseModel
{
    [Required]
    public int RolId { get; set; }

    [Required]
    public int FormId { get; set; }

    [Required]
    public int PermissionId { get; set; }

    [ForeignKey(nameof(RolId))]
    public Rol Rol { get; set; }

    [ForeignKey(nameof(FormId))]
    public Form Form { get; set; }

    [ForeignKey(nameof(PermissionId))]
    public Permission Permission { get; set; }
}
