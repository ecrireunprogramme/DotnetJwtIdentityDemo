using System.ComponentModel.DataAnnotations;

namespace DotnetJwtIdentityDemo.DataTransfertObjects
{
    public class UserRegistrationDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}
