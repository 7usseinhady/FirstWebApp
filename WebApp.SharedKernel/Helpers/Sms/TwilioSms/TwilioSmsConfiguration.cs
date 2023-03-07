
namespace WebApp.SharedKernel.Helpers.Sms.TwilioSms
{
    public class TwilioSmsConfiguration
    {
        public string AccountSID { get; set; } = default!;
        public string AuthToken { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}
