using Newtonsoft.Json.Linq;

namespace WebApp.SharedKernel.Interfaces
{
    public interface IPaymentService<TPaymentRequestDTO> where TPaymentRequestDTO : class
    {
        Task<JObject> PayAsync(TPaymentRequestDTO paymentRequestDTO);
    }
}
