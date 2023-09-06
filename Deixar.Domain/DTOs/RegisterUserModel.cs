using System.ComponentModel.DataAnnotations;

namespace Deixar.Domain.DTOs
{
    public class RegisterUserModel
    {
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        public string? Address { get; set; }

        public string Role { get; set; } = null!;
    }
}
