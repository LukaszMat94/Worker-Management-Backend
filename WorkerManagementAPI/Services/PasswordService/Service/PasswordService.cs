using System.Text;

namespace WorkerManagementAPI.Services.PasswordService.Service
{
    public class PasswordService : IPasswordService
    {
        private List<string> PasswordCharacters { get; set; } = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
        "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private const int TemporaryPasswordLength = 8;

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
    }
}
