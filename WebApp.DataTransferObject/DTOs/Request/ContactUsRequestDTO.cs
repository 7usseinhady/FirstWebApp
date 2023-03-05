namespace WebApp.DataTransferObject.DTOs.Request
{
    public class ContactUsRequestDTO
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AddressAr { get; set; }
        public string AddressEn { get; set; }
        public string? WebsiteLink { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? TwitterLink { get; set; }
    }
}
