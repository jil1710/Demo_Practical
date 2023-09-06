using System.ComponentModel.DataAnnotations.Schema;

namespace Deixar.Domain.Entities;

public class Role
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string RoleName { get; set; }

    //Relational properties
    public ICollection<UserRole> Users { get; set; }
}
