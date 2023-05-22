using System.ComponentModel.DataAnnotations;

namespace DotnetJwtIdentityDemo.DataTransfertObjects
{
    public class UserLoginDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
