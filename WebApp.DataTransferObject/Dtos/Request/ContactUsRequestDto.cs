namespace WebApp.DataTransferObject.Dtos.Request
{
    public class ContactUsRequestDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string AddressAr { get; set; } = default!;
        public string AddressEn { get; set; } = default!;
        public string? WebsiteLink { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? TwitterLink { get; set; }
    }
}
