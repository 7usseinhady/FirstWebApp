
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.DTOs.Request
{
    public class ReturnResponseDTO
    {
        public string cartId { get; set; }
        public string tranRef { get; set; }
        public string customerEmail { get; set; }

        public string respStatus { get; set; }
        public string respCode { get; set; }
        public string respMessage { get; set; }


        public string acquirerMessage { get; set; }
        public string acquirerRRN { get; set; }
        public string token { get; set; }

    }
}
