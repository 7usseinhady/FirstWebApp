
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.Dtos
{
    public class PaymentResultDto
    {
        public string response_status { get; set; } = default!;
        public string response_code { get; set; } = default!;
        public string response_message { get; set; } = default!;
        public DateTime transaction_time { get; set; } = default!;
    }
}
