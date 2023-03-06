using Microsoft.AspNetCore.Http;

namespace WebApp.SharedKernel.Dtos
{
    public class FileDto
    {
        public string? Id { get; set; }
        public IFormFile? File { get; set; }
        public string? FilePath { get; set; }

    }
}
