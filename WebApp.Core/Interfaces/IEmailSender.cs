using WebApp.Core.Helpers.Email;

namespace WebApp.Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
