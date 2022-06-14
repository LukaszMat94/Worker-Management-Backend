using Microsoft.AspNetCore.Identity;
using System.Text;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.UserDtos;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.ExceptionsTemplate;

namespace WorkerManagementAPI.Services.PasswordService.Service
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public PasswordService(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public void HashPassword(User user, String? password)
        {
            user.Password = _passwordHasher.HashPassword(user, password);
        }

        public void VerifyPassword(User user, LoginUserDto loginUserDto)
        {
            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);

            if(verificationResult == PasswordVerificationResult.Failed)
            {
                throw new PasswordNotMatchException(ExceptionCodeTemplate.BCKND_PASSWORD_NOT_MATCH_NOTFOUND);
            }
        }
    }
}
