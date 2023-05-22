using DotnetJwtIdentityDemo.DataTransfertObjects;
using DotnetJwtIdentityDemo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DotnetJwtIdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthenticationRepository _userAuthRepo;

        public AuthController(IUserAuthenticationRepository userAuthRepo)
        {
            _userAuthRepo = userAuthRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _userAuthRepo.RegisterUserAsync(userRegistrationDto);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _userAuthRepo.ValidateUserAsync(userLoginDto))
                return Unauthorized();

            return Ok(new { token = await _userAuthRepo.CreateTokenAsync() });
        }
    }
}
