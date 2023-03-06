using WebApp.DataTransferObject.Dtos.Request;

namespace WebApp.DataTransferObject.Dtos.Response
{
    public class TermResponseDto : TermRequestDto
    {
        public string? TermTitle { get; set; }
        public string? TermBody { get; set; }
    }
}
