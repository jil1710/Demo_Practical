using Deixar.Domain.Entities;

namespace Deixar.Domain.DTOs
{
    /// <summary>
    /// Contains full details of user including their roles 
    /// </summary>
    public class UserDetails
    {
        public User User { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
