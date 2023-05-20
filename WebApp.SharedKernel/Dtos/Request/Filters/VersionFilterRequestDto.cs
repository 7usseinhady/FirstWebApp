using WebApp.SharedKernel.Bases;

namespace WebApp.SharedKernel.Dtos.Request.Filters
{
    public class VersionFilterRequestDto : BaseFilterRequestDto
    {
        public int? Id { get; set; }
        public string? VersionCode { get; set; }
        public int? ForcedUpdate { get; set; }

    }
}
