using Microsoft.AspNetCore.Http;

namespace WebApp.SharedKernel.DTOs
{
    public class FileDTO
    {
        public string? Id { get; set; }
        public IFormFile? File { get; set; }
        public string? FilePath { get; set; }

    }
}
