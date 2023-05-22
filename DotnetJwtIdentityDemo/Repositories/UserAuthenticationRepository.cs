using AutoMapper;
using DotnetJwtIdentityDemo.DataTransfertObjects;
using DotnetJwtIdentityDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotnetJwtIdentityDemo.Repositories
{
    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private User? _user;

        public UserAuthenticationRepository(UserManager<User> userManager, 
            IMapper mapper,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
        {
            var user = _mapper.Map<User>(userRegistrationDto);

            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);

            if (result.Succeeded)
                await _userManager.AddToRolesAsync(user, userRegistrationDto.Roles);

            return result;
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto userLoginDto)
        {
            _user = await _userManager.FindByNameAsync(userLoginDto.UserName);

            return _user != null && await _userManager.CheckPasswordAsync(_user, userLoginDto.Password);
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("ValidIssuer").Value,
                audience: jwtSettings.GetSection("ValidAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("ExpiresIn").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings.GetSection("SecretKey").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
