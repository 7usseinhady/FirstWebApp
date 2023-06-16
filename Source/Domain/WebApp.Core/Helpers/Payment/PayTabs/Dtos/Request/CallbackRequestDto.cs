
namespace WebApp.Core.Helpers.Payment.PayTabs.Dtos.Request
{
    public class CallbackResponseDto
    {
        public string cart_id { get; set; } = default!;
        public string tran_ref { get; set; } = default!;
        public string cart_amount { get; set; } = default!;
        public string cart_currency { get; set; } = default!;
        public string cart_description { get; set; } = default!;

        public CustomerORShippingDetailsDto customer_details { get; set; } = default!;
        public CustomerORShippingDetailsDto shipping_details { get; set; } = default!;

        public PaymentResultDto payment_result { get; set; } = default!;
        public PaymentInfoDto payment_info { get; set; } = default!;
    }
}
