
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.Dtos.Request
{
    public class PaymentInitialRequestDto
    {
        public string cart_id { get; set; } = default!;
        public decimal cart_amount { get; set; } = default!;
        public CustomerORShippingDetailsDto customer_details { get; set; } = default!;
        public string callback { get; set; } = default!;
        public string @return { get; set; } = default!;

        public int profile_id { get; } = 00000;
        public string tran_type { get; } = "sale";
        public string cart_description { get; } = "cart_description";
        public string cart_currency { get; } = "SAR";
        public string tran_class { get; set; } = "ecom";


        public bool hide_shipping { get; } = true;
        public string paypage_lang { get; } = "ar";
    }
}
