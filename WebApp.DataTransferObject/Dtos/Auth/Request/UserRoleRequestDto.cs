using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObject.Dtos.Auth.Request
{
    public class UserRoleRequestDto
    {
        [Required]
        public string UserId { get; set; } = default!;

        [Required]
        public string RoleId { get; set; } = default!;

    }
}
