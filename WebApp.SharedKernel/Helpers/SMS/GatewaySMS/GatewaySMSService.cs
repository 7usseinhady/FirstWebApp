using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using Newtonsoft.Json.Linq;

namespace WebApp.SharedKernel.Helpers.SMS.GatewaySMS
{
    public class GatewaySMSService : ISMSService
    {
        private GatewaySMSRequest gatewaySMSRequest;
        private readonly IBaseApiConnection _baseApiConnection;
        public GatewaySMSService(GatewaySMSConfiguration gatewaySMSConfiguration, IBaseApiConnection baseApiConnection)
        {
            gatewaySMSRequest = new GatewaySMSRequest() {
                api_id = gatewaySMSConfiguration.api_id,
                api_password = gatewaySMSConfiguration.api_password,
                sender_id = gatewaySMSConfiguration.sender_id,
                templateid = gatewaySMSConfiguration.templateid
            };

            _baseApiConnection = baseApiConnection;
            _baseApiConnection.SetApiUri(Res.GatewaySMSUri);
        }
        public async Task SendAsync(string mobileNumber, string body)
        {
            try
            {
                gatewaySMSRequest.phonenumber = mobileNumber.StartsWith("+") ? mobileNumber.Remove(0, 1) : mobileNumber;
                gatewaySMSRequest.V1 = body;
                JObject result = await _baseApiConnection.PostAsync(controller: "SendSMS", body: gatewaySMSRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendMultiAsync(List<string> lMobileNumbers, string body)
        {
            try
            {
                lMobileNumbers = lMobileNumbers.Select(x => x.StartsWith("+") ? x.Remove(0, 1) : x).ToList();
                gatewaySMSRequest.phonenumber = string.Join(",", lMobileNumbers);
                gatewaySMSRequest.V1 = body;
                JObject result = await _baseApiConnection.PostAsync(controller: "SendSMSMulti", body: gatewaySMSRequest).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
