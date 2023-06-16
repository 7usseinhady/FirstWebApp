using WebApp.SharedKernel.Dtos.Request;

namespace WebApp.SharedKernel.Dtos.Response
{
    public class TermResponseDto : TermRequestDto
    {
        public string? TermTitle { get; set; }
        public string? TermBody { get; set; }
    }
}
