
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.Dtos
{
    public class CustomerORShippingDetailsDto
    {
        public string name { get; set; } = default!;
        public string email { get; set; } = default!;
        public string phone { get; set; } = default!;
        public string ip { get; set; } = default!;

        public string street1 { get; set; } = "none";
        public string city { get; set; } = "RUH";
        public string state { get; set; } = "01";
        public string country { get; set;  } = "sa";
        
    }
}
