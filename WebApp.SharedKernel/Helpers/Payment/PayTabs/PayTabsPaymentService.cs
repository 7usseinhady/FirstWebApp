using Newtonsoft.Json.Linq;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using WebApp.SharedKernel.Helpers.Payment.PayTabs.DTOs.Request;

namespace WebApp.SharedKernel.Helpers.Payment.PayTabs
{
    public class PayTabsPaymentService : IPaymentService<PaymentInitialRequestDTO>
    {
        private readonly IBaseApiConnection _baseApiConnection;

        public PayTabsPaymentService(IBaseApiConnection baseApiConnection)
        {
            _baseApiConnection = baseApiConnection;
            _baseApiConnection.SetApiUri(Res.PayTabsUri);
        }

        public async Task<JObject> PayAsync(PaymentInitialRequestDTO paymentInitialRequestDTO)
        {
            try
            {
                JObject result = null;
                return await _baseApiConnection.PostAsync(controller: "payment", action: "request", body: paymentInitialRequestDTO);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
