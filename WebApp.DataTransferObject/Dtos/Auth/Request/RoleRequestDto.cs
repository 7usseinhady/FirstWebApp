
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.Dtos.Auth.Request
{
    public class RoleRequestDto
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required]
        public string Name { get; set; } = default!;

    }
}
