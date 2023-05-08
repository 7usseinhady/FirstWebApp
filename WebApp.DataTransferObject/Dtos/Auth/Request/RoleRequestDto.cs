
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObject.Dtos.Auth.Request
{
    public class RoleRequestDto
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required]
        public string Name { get; set; } = default!;

    }
}
