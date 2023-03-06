
namespace WebApp.SharedKernel.Helpers.Payment.PayTabs.Dtos.Request
{
    public class ReturnResponseDto
    {
        public string cartId { get; set; } = default!;
        public string tranRef { get; set; } = default!;
        public string customerEmail { get; set; } = default!;

        public string respStatus { get; set; } = default!;
        public string respCode { get; set; } = default!;
        public string respMessage { get; set; } = default!;


        public string acquirerMessage { get; set; } = default!;
        public string acquirerRRN { get; set; } = default!;
        public string token { get; set; } = default!;

    }
}
