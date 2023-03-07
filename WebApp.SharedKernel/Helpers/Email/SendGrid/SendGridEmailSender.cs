using WebApp.SharedKernel.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace WebApp.SharedKernel.Helpers.Email.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridKeyConfiguration _keyConfig;
        private readonly IFileUtils _fileUtils;

        public SendGridEmailSender(SendGridKeyConfiguration keyConfig, IFileUtils fileUtils)
        {
            _keyConfig = keyConfig;
            _fileUtils = fileUtils;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var sendGridMessage = CreateEmailMessage(message);
            
            // From
            sendGridMessage.SetFrom(new EmailAddress(_keyConfig.Email, _keyConfig.Name));

            var apiKey = _keyConfig.SendGridKey;
            var client = new SendGridClient(apiKey);
            await client.SendEmailAsync(sendGridMessage);
        }

        private SendGridMessage CreateEmailMessage(EmailMessage message)
        {
            var sendGridMessage = new SendGridMessage();

            //To
            int i = 0;
            foreach (var to in message.tos)
            {
                sendGridMessage.AddTo(to, i.ToString());
                i++;
            }

            // Subject
            sendGridMessage.SetGlobalSubject(message.subject);

            // Body
            if (!string.IsNullOrEmpty(message.body))
            {
                var htmlContent = string.Format("<p>{0}<p>", message.body);
                sendGridMessage.AddContent(MimeType.Html, htmlContent);
            }

            // Attachments
            var SendGridAttachments = new List<Attachment>();
            if (message.attachments is not null && message.attachments.Any())
            {
                foreach (var file in message.attachments)
                {
                    if (file.Length > 0)
                    {
                        var attachment = new Attachment();
                        attachment.Content = _fileUtils.ToBase64String(file);
                        attachment.Type = file.ContentType;
                        attachment.Filename = file.FileName;
                        attachment.Disposition = file.ContentDisposition;
                        attachment.ContentId = file.FileName;
                        SendGridAttachments.Add(attachment);
                    }
                }
                sendGridMessage.AddAttachments(SendGridAttachments);
            }
            return sendGridMessage;
        }

    }
}
