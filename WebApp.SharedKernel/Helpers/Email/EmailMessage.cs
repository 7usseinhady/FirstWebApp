using Microsoft.AspNetCore.Http;

namespace WebApp.SharedKernel.Helpers.Email
{
    public class EmailMessage
    {
        public List<string> tos { get; private set; }
        public string subject { get; private set; }
        public string body { get; private set; }
        public IFormFileCollection? attachments { get; private set; }

        public EmailMessage(List<string> tos, string subject, string body, IFormFileCollection attachments = null!)
        {
            this.tos = tos;
            this.subject = subject;
            this.body = body;
            this.attachments = attachments;
        }
    }
}
