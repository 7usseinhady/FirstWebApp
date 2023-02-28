using WebApp.DataTransferObject.DTOs.Request;

namespace WebApp.DataTransferObject.DTOs.Response
{
    public class ContactUsResponseDTO : ContactUsRequestDTO
    {
        public string? Address { get; set; }
    }
}
