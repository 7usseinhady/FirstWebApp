using WebApp.SharedKernel.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace WebApp.SharedKernel.Helpers.Sms.TwilioSms
{
    public class TwilioSmsService : ISmsService
    {
        private readonly TwilioSmsConfiguration _twilioSmsConfiguration;
        public TwilioSmsService(TwilioSmsConfiguration twilioSmsConfiguration)
        {
            _twilioSmsConfiguration = twilioSmsConfiguration;
            TwilioClient.Init(_twilioSmsConfiguration.AccountSID, _twilioSmsConfiguration.AuthToken);
        }
        public async Task SendAsync(string mobileNumber, string body)
        {
            _ = MessageResource.Create(
                from: new Twilio.Types.PhoneNumber(_twilioSmsConfiguration.PhoneNumber),
                body: body,
                to: mobileNumber);
        }

        public async Task SendMultiAsync(List<string> lMobileNumbers, string body)
        {
            foreach (var mobileNumber in lMobileNumbers)
                await SendAsync(mobileNumber, body);
        }
    }
}
