
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.Dtos.Request
{
    public class CallbackResponseDto
    {
        public string cart_id { get; set; }
        public string tran_ref { get; set; }
        public string cart_amount { get; set; }
        public string cart_currency { get; set; }
        public string cart_description { get; set; }

        public CustomerORShippingDetailsDto customer_details { get; set; }
        public CustomerORShippingDetailsDto shipping_details { get; set; }

        public PaymentResultDto payment_result { get; set; }
        public PaymentInfoDto payment_info { get; set; }
    }
}
