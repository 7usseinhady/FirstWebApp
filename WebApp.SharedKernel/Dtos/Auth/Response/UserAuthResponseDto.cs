using WebApp.SharedKernel.Interfaces;
using Newtonsoft.Json;

namespace WebApp.SharedKernel.Dtos.Auth.Response
{
    public class UserAuthResponseDto : IFilePathDto
    {
        public string Id { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<string> Roles { get; set; } = default!;

        public string? RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime RefreshTokenExpiration { get; set; }

        public string AccessToken { get; set; } = default!;

        public DateTime AccessTokenExpiration { get; set; }

        public string? Path { get; set; }
        public string? DisplayPath { get; set; }
    }
}
