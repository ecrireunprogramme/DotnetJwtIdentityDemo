using DotnetJwtIdentityDemo.DataTransfertObjects;
using Microsoft.AspNetCore.Identity;

namespace DotnetJwtIdentityDemo.Repositories
{
    public interface IUserAuthenticationRepository
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
        Task<bool> ValidateUserAsync(UserLoginDto userLoginDto);
        Task<string> CreateTokenAsync();
    }
}
