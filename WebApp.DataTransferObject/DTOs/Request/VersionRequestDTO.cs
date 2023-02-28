using System.ComponentModel.DataAnnotations;
using WebApp.DataTransferObjects.DTOs;

namespace WebApp.DataTransferObject.DTOs.Request
{
    public class VersionRequestDTO
    {
        public int Id { get; set; }
        public string? VersionCode { get; set; }
        public SystemVersionDTO? SystemVersion { get; set; }
        public bool ForcedUpdate { get; set; }

    }
}
