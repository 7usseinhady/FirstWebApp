using WebApp.DataTransferObjects.Interfaces;
using Newtonsoft.Json;

namespace WebApp.DataTransferObjects.Dtos.Auth.Response
{
    public class UserAuthResponseDto : IFilePathDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }

        public string? RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime RefreshTokenExpiration { get; set; }

        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiration { get; set; }

        public string? Path { get; set; }
        public string? DisplayPath { get; set; }
    }
}
