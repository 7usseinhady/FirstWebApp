using WebApp.SharedKernel.Helpers.Email;

namespace WebApp.SharedKernel.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
