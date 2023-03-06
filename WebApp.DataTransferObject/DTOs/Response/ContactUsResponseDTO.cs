using WebApp.DataTransferObject.Dtos.Request;

namespace WebApp.DataTransferObject.Dtos.Response
{
    public class ContactUsResponseDto : ContactUsRequestDto
    {
        public string? Address { get; set; }
    }
}
