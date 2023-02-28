
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.DTOs.Request
{
    public class PaymentInitialRequestDTO
    {
        public string cart_id { get; set; }
        public decimal cart_amount { get; set; }
        public CustomerORShippingDetailsDTO customer_details { get; set; }
        public string callback { get; set; }
        public string @return { get; set; }

        public int profile_id { get; } = 00000; //90218;
        public string tran_type { get; } = "sale";
        public string cart_description { get; } = "cart_description";
        public string cart_currency { get; } = "SAR";
        public string tran_class { get; set; } = "ecom";


        public bool hide_shipping { get; } = true;
        public string paypage_lang { get; } = "ar";
    }
}
