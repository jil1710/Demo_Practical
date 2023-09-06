using System.ComponentModel;

namespace Deixar.Domain.DTOs
{
    public class Credentials
    {
        public string EmailAddress { get; set; } = null!;

        public string Password { get; set; } = null!;

        [DisplayName("Remeber Me")]
        public bool RememberMe { get; set; }
    }
}
