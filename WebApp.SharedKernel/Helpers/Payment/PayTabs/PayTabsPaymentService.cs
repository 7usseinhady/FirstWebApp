using Newtonsoft.Json.Linq;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using WebApp.SharedKernel.Helpers.Payment.PayTabs.Dtos.Request;

namespace WebApp.SharedKernel.Helpers.Payment.PayTabs
{
    public class PayTabsPaymentService : IPaymentService<PaymentInitialRequestDto>
    {
        private readonly IBaseApiConnection _baseApiConnection;

        public PayTabsPaymentService(IBaseApiConnection baseApiConnection)
        {
            _baseApiConnection = baseApiConnection;
            _baseApiConnection.SetApiUri(Res.PayTabsUri);
        }

        public async Task<JObject> PayAsync(PaymentInitialRequestDto paymentInitialRequestDto)
        {
            try
            {
                JObject result = null;
                return await _baseApiConnection.PostAsync(controller: "payment", action: "request", body: paymentInitialRequestDto);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
