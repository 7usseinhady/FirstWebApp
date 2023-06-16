using WebApp.SharedKernel.Bases;

namespace WebApp.SharedKernel.Dtos.Request.Filters
{
    public class ContactUsFilterRequestDto : BaseFilterRequestDto
    {
        public int? ContactUsId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
