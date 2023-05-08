namespace WebApp.DataTransferObject.Filters
{
    public class ContactUsFilter : PagerFilter
    {
        public int? ContactUsId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
