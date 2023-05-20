using Newtonsoft.Json.Linq;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;
using WebApp.Core.Helpers.Payment.PayTabs.Dtos.Request;
using System.Runtime.ExceptionServices;
using WebApp.Core.Interfaces;

namespace WebApp.Core.Helpers.Payment.PayTabs
{
    public class PayTabsPaymentService : IPaymentService<PaymentInitialRequestDto>
    {
        private readonly IBaseApiConnection _baseApiConnection;

        public PayTabsPaymentService(IBaseApiConnection baseApiConnection)
        {
            _baseApiConnection = baseApiConnection;
            _baseApiConnection.SetApiUri(Res.payTabsUri);
        }

        public async Task<JObject> PayAsync(PaymentInitialRequestDto paymentInitialRequestDto)
        {
            try
            {
                return await _baseApiConnection.PostAsync(controller: "payment", action: "request", body: paymentInitialRequestDto);
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                return null!;
            }
        }
    }
}
