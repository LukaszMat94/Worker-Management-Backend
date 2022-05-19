namespace WorkerManagementAPI.Data.MailConfig
{
    public class MailSettings
    {
        public string Mail { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Host { get; set; } = String.Empty;
        public int Port { get; set; }
    }
}
