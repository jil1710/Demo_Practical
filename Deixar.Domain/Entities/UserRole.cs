namespace Deixar.Domain.Entities;

public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    //Navigation properties
    public Role? Role { get; set; }
    public User? User { get; set; }
}
