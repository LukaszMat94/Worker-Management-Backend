using Microsoft.AspNetCore.Identity;
using System.Text;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.UserDtos;
using WorkerManagementAPI.Exceptions;

namespace WorkerManagementAPI.Services.PasswordService.Service
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private List<string> PasswordCharacters { get; set; } = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
        "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private const int TemporaryPasswordLength = 8;

        public PasswordService(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public void HashPassword(User user, String password)
        {
            user.Password = _passwordHasher.HashPassword(user, password);
        }

        public string GenerateTemporaryPassword()
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
    }
}
