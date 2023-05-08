namespace WebApp.SharedKernel.Filters
{
    public class VersionFilter : PagerFilter
    {
        public int? Id { get; set; }
        public string? VersionCode { get; set; }
        public int? ForcedUpdate { get; set; }

    }
}
