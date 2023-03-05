
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.DTOs.Response
{
    public class PaymentInitialResponseDTO
    {
        public string cart_id { get; set; }
        public string tran_ref { get; set; }
        public string cart_amount { get; set; }
        public CustomerORShippingDetailsDTO customer_details { get; set; }
        public string callback { get; set; }
        public string @return { get; set; }

        public int profileId { get; set; }
        public string tran_type { get; set; }
        public string cart_description { get; set; }
        public string cart_currency { get; set; }


        public string redirect_url { get; set; }

        public int serviceId { get; set; }
        public int merchantId { get; set; }
        public string trace { get; set; }
    }
}
