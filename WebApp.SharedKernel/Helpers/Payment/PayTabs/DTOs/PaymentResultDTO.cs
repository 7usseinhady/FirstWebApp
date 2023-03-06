
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.Dtos
{
    public class PaymentResultDto
    {
        public string response_status { get; set; }
        public string response_code { get; set; }
        public string response_message { get; set; }
        public DateTime transaction_time { get; set; }
    }
}
