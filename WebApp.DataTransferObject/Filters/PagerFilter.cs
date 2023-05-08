namespace WebApp.DataTransferObject.Filters
{
    public class PagerFilter
    {
        public int? PageSize { get; set; }

        public int? CurrentPage { get; set; }

        public int MaxPaginationWidth { get; set; }

    }
}
