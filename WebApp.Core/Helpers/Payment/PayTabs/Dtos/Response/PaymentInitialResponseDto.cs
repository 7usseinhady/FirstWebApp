
namespace WebApp.Core.Helpers.Payment.PayTabs.Dtos.Response
{
    public class PaymentInitialResponseDto
    {
        public string cart_id { get; set; } = default!;
        public string tran_ref { get; set; } = default!;
        public string cart_amount { get; set; } = default!;
        public CustomerORShippingDetailsDto customer_details { get; set; } = default!;
        public string callback { get; set; } = default!;
        public string @return { get; set; } = default!;

        public int profileId { get; set; } = default!;
        public string tran_type { get; set; } = default!;
        public string cart_description { get; set; } = default!;
        public string cart_currency { get; set; } = default!;


        public string redirect_url { get; set; } = default!;

        public int serviceId { get; set; } = default!;
        public int merchantId { get; set; } = default!;
        public string trace { get; set; } = default!;
    }
}
