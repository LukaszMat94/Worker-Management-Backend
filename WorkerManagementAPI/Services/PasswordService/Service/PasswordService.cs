using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.JwtToken;
using WorkerManagementAPI.Data.Models.UserDtos;
using WorkerManagementAPI.Exceptions;

namespace WorkerManagementAPI.Services.PasswordService.Service
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtAuthenticationSettings _jwtAuthenticationSettings;
        private List<string> PasswordCharacters { get; set; } = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
        "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private const int TemporaryPasswordLength = 8;

        public PasswordService(IPasswordHasher<User> passwordHasher,
            JwtAuthenticationSettings jwtAuthenticationSettings)
        {
            _passwordHasher = passwordHasher;
            _jwtAuthenticationSettings = jwtAuthenticationSettings;
        }

        public void HashPassword(User user)
        {
            string password = GenerateTemporaryPassword();
            user.Password = _passwordHasher.HashPassword(user, password);
        }

        private string GenerateTemporaryPassword()
        {
            StringBuilder passwordBuilder = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < TemporaryPasswordLength; i++)
            {
                int index = random.Next(0, PasswordCharacters.Count - 1);

                passwordBuilder.Append(PasswordCharacters[index]);
            };

            return passwordBuilder.ToString();
        }

        public void VerifyPassword(User user, LoginUserDto loginUserDto)
        {
            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);

            if(verificationResult == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Password dont match to this account");
            }
        }

        public string GenerateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new Claim(ClaimTypes.Role, $"{user.Role.RoleName}")
            };

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthenticationSettings.JwtKey));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime expireDay = DateTime.Now.AddDays(_jwtAuthenticationSettings.JwtExpireDays);

            JwtSecurityToken token = new JwtSecurityToken(_jwtAuthenticationSettings.JwtIssuer, 
                _jwtAuthenticationSettings.JwtIssuer, 
                claims, 
                expires: expireDay, 
                signingCredentials: credentials);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
