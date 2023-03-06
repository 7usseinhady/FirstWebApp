using WebApp.DataTransferObjects.Helpers;

namespace WebApp.DataTransferObjects.Filters
{
    public class VersionFilter : Pager
    {
        public int? Id { get; set; }
        public string? VersionCode { get; set; }
        public int? ForcedUpdate { get; set; }

    }
}
