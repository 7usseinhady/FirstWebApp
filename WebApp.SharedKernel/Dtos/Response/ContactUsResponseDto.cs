using WebApp.SharedKernel.Dtos.Request;

namespace WebApp.SharedKernel.Dtos.Response
{
    public class ContactUsResponseDto : ContactUsRequestDto
    {
        public string? Address { get; set; }
    }
}
