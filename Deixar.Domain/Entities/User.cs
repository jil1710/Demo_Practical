using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deixar.Domain.Entities;

public class User
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? MiddleName { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    [Column(TypeName = "nvarchar(260)")]
    public string EmailAddress { get; set; }

    [DataType(DataType.Password)]
    [Column(TypeName = "nvarchar(128)")]
    public string Password { get; set; }

    [DataType(DataType.PhoneNumber)]
    [Column(TypeName = "varchar(10)")]
    public string ContactNumber { get; set; }

    [Column(TypeName = "nvarchar(250)")]
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;

    //Relational properties
    public ICollection<UserRole> Roles { get; set; }

}
