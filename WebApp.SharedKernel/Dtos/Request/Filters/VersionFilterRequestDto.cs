namespace WebApp.SharedKernel.Dtos.Request.Filters
{
    public class VersionFilterRequestDto : PagerFilterRequestDto
    {
        public int? Id { get; set; }
        public string? VersionCode { get; set; }
        public int? ForcedUpdate { get; set; }

    }
}
