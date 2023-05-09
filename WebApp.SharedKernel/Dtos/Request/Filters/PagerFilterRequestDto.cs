namespace WebApp.SharedKernel.Dtos.Request.Filters
{
    public class PagerFilterRequestDto
    {
        public int? PageSize { get; set; }

        public int? CurrentPage { get; set; }

        public int MaxPaginationWidth { get; set; }

    }
}
