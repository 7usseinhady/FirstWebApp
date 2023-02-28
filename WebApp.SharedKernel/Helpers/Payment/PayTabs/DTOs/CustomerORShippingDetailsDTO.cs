
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.DTOs
{
    public class CustomerORShippingDetailsDTO
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string ip { get; set; }

        public string street1 { get; set; } = "none";
        public string city { get; set; } = "RUH";
        public string state { get; set; } = "01";
        public string country { get; set;  } = "sa";
        
    }
}
