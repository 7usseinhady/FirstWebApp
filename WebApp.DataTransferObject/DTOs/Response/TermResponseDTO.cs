using WebApp.DataTransferObject.DTOs.Request;

namespace WebApp.DataTransferObject.DTOs.Response
{
    public class TermResponseDTO : TermRequestDTO
    {
        public string? TermTitle { get; set; }
        public string? TermBody { get; set; }
    }
}
