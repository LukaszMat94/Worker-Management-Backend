using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using WorkerManagementAPI.Data.MailConfig;

namespace WorkerManagementAPI.Services.MailService.Service
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string emailUser, string? password)
        {
            MimeMessage emailMessage = SetEmailMessage(emailUser, password);

            using SmtpClient smtpClient = new SmtpClient();

            smtpClient.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

            smtpClient.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtpClient.SendAsync(emailMessage);

            smtpClient.Disconnect(true);
        }

        private MimeMessage SetEmailMessage(string emailUser, string? password)
        {
            MimeMessage email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

            email.To.Add(MailboxAddress.Parse(emailUser));

            email.Subject = "Welcome in Worker Management API";

            email.Body = new TextPart("plain")
            {
                Text = $"Hello, this is your temporary password: {password}. Please change it after first login. Your account will be inactive until the change of password"
            };

            return email;
        }
    }
}
