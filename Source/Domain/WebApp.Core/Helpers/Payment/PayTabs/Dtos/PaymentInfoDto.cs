
namespace WebApp.Core.Helpers.Payment.PayTabs.Dtos
{
    public class PaymentInfoDto
    {
        public string card_type { get; set; } = default!;
        public string card_scheme { get; set; } = default!;
        public string payment_description { get; set; } = default!;
    }
}
