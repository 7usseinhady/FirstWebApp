using WebApp.SharedKernel.Bases;

namespace WebApp.SharedKernel.Dtos.Request.Filters
{
    public class TermFilterRequestDto : BaseFilterRequestDto
    {
        public int? Id { get; set; }
        public string? TermTitle { get; set; }


    }
}
