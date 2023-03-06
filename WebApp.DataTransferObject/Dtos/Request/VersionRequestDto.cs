using System.ComponentModel.DataAnnotations;
using WebApp.DataTransferObjects.Dtos;

namespace WebApp.DataTransferObject.Dtos.Request
{
    public class VersionRequestDto
    {
        public int Id { get; set; } = default!;
        public string? VersionCode { get; set; }
        public SystemVersionDto? SystemVersion { get; set; }
        public bool ForcedUpdate { get; set; } = default!;

    }
}
