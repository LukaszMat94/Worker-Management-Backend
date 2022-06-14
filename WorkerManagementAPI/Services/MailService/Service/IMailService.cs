namespace WorkerManagementAPI.Services.MailService.Service
{
    public interface IMailService
    {
        Task SendEmailAsync(string email, string? password);
    }
}
