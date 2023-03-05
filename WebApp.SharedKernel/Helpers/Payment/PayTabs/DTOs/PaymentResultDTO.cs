
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.DTOs
{
    public class PaymentResultDTO
    {
        public string response_status { get; set; }
        public string response_code { get; set; }
        public string response_message { get; set; }
        public DateTime transaction_time { get; set; }
    }
}
