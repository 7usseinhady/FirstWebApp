namespace WebApp.SharedKernel.Bases
{
    public abstract class BaseFilterRequestDto
    {
        public int? PageSize { get; set; }

        public int? CurrentPage { get; set; }

        public int MaxPaginationWidth { get; set; }

    }
}
