using WebApp.SharedKernel.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace WebApp.SharedKernel.Helpers.SMS.TwilioSMS
{
    public class TwilioSMSService : ISMSService
    {
        private readonly TwilioSMSConfiguration _twilioSMSConfiguration;
        public TwilioSMSService(TwilioSMSConfiguration twilioSMSConfiguration)
        {
            _twilioSMSConfiguration = twilioSMSConfiguration;
            TwilioClient.Init(_twilioSMSConfiguration.AccountSID, _twilioSMSConfiguration.AuthToken);
        }
        public async Task SendAsync(string mobileNumber, string body)
        {
            var result = MessageResource.Create(
                from: new Twilio.Types.PhoneNumber(_twilioSMSConfiguration.PhoneNumber),
                body: body,
                to: mobileNumber);
        }

        public async Task SendMultiAsync(List<string> lMobileNumbers, string body)
        {
            foreach (var mobileNumber in lMobileNumbers)
                SendAsync(mobileNumber, body);
        }
    }
}
