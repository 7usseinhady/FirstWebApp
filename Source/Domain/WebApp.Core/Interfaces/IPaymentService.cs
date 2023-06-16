using Newtonsoft.Json.Linq;

namespace WebApp.Core.Interfaces
{
    public interface IPaymentService<TPaymentRequestDto> where TPaymentRequestDto : class
    {
        Task<JObject> PayAsync(TPaymentRequestDto paymentRequestDto);
    }
}
