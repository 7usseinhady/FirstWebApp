using WebApp.SharedKernel.Interfaces;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using System.Runtime.ExceptionServices;

namespace WebApp.SharedKernel.Helpers.Email.MailKit
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly MailKitEmailConfiguration _emailConfig;

        public MailKitEmailSender(MailKitEmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var mailMessage = CreateEmailMessage(message);

            mailMessage.From.Add(new MailboxAddress(name: _emailConfig.From, address: _emailConfig.From));

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.None);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    _ = await client.SendAsync(mailMessage);
                }
                catch (AggregateException ex)
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();

            //To
            emailMessage.To.AddRange(message.tos.Select(x => new MailboxAddress(name: x, address: x)));

            // Subject
            emailMessage.Subject = message.subject;

            // Body
            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<p>{0}<p>", message.body) };

            // Attachments
            if (message.attachments != null && message.attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }


    }
}
