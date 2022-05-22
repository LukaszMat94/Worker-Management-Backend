using System.Text;
using WorkerManagementAPI.Data.Entities.Enums;
using WorkerManagementAPI.Data.JwtToken;

namespace WorkerManagementAPI.Data.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string? Password { get ; set; }
        public long? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public virtual List<Technology>? Technologies { get; set; } = new List<Technology>();
        public virtual List<Project>? Projects { get; set; } = new List<Project>();
        public long RoleId { get; set; }
        public virtual Role Role { get; set; }
        public AccountStatusEnum AccountStatus { get; set; } = AccountStatusEnum.INACTIVE;
        public virtual List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken> ();

        public User()
        {
            Password = GenerateTemporaryPassword();
        }

        private string GenerateTemporaryPassword()
        {
            int passwordLength = 8;

            //ASCII codes for numbers 0-9
            int[] numberArray = Enumerable.Range(48, 10).ToArray();
            //ASCII codes for letters a-z
            int[] lowercaseLetterArray = Enumerable.Range(65, 26).ToArray();
            //ASCII codes for letters A_Z
            int[] uppercaseLetterArray = Enumerable.Range(97, 26).ToArray();

            int[] result = numberArray.Concat(lowercaseLetterArray).Concat(uppercaseLetterArray).ToArray();

            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                int index = random.Next(0, result.Length - 1);
                builder.Append((char) result[index]);
            }

            return builder.ToString();
        }
    }
}