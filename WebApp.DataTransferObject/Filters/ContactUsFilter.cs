using WebApp.DataTransferObjects.Helpers;

namespace WebApp.DataTransferObjects.Filters
{
    public class ContactUsFilter : Pager
    {
        public int? ContactUsId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
