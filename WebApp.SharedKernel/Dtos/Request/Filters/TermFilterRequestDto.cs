namespace WebApp.SharedKernel.Dtos.Request.Filters
{
    public class TermFilterRequestDto : PagerFilterRequestDto
    {
        public int? Id { get; set; }
        public string? TermTitle { get; set; }


    }
}
