
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.DTOs.Request
{
    public class CallbackResponseDTO
    {
        public string cart_id { get; set; }
        public string tran_ref { get; set; }
        public string cart_amount { get; set; }
        public string cart_currency { get; set; }
        public string cart_description { get; set; }

        public CustomerORShippingDetailsDTO customer_details { get; set; }
        public CustomerORShippingDetailsDTO shipping_details { get; set; }

        public PaymentResultDTO payment_result { get; set; }
        public PaymentInfoDTO payment_info { get; set; }
    }
}
