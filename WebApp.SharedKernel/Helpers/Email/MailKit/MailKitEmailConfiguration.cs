namespace WebApp.SharedKernel.Helpers.Email.MailKit
{
    public class MailKitEmailConfiguration
    {
        public string From { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public int Port { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
