using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using System.Runtime.ExceptionServices;
using WebApp.Core.Interfaces;

namespace WebApp.Core.Helpers.Sms.GatewaySms
{
    public class GatewaySmsService : ISmsService
    {
        private readonly GatewaySmsRequest gatewaySmsRequest;
        private readonly IBaseApiConnection _baseApiConnection;
        public GatewaySmsService(GatewaySmsConfiguration gatewaySmsConfiguration, IBaseApiConnection baseApiConnection)
        {
            gatewaySmsRequest = new GatewaySmsRequest() {
                api_id = gatewaySmsConfiguration.api_id,
                api_password = gatewaySmsConfiguration.api_password,
                sender_id = gatewaySmsConfiguration.sender_id,
                templateid = gatewaySmsConfiguration.templateid
            };

            _baseApiConnection = baseApiConnection;
            _baseApiConnection.SetApiUri(Res.gatewaySmsUri);
        }
        public async Task SendAsync(string mobileNumber, string body)
        {
            try
            {
                gatewaySmsRequest.phonenumber = mobileNumber.StartsWith("+") ? mobileNumber.Remove(0, 1) : mobileNumber;
                gatewaySmsRequest.V1 = body;
                _ = await _baseApiConnection.PostAsync(controller: "SendSms", body: gatewaySmsRequest).ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }

        public async Task SendMultiAsync(List<string> lMobileNumbers, string body)
        {
            try
            {
                lMobileNumbers = lMobileNumbers.Select(x => x.StartsWith("+") ? x.Remove(0, 1) : x).ToList();
                gatewaySmsRequest.phonenumber = string.Join(",", lMobileNumbers);
                gatewaySmsRequest.V1 = body;
                _ = await _baseApiConnection.PostAsync(controller: "SendSmsMulti", body: gatewaySmsRequest).ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
        }
    }
}
