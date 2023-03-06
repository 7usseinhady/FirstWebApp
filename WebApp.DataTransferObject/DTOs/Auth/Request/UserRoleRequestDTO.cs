using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.Dtos.Auth.Request
{
    public class UserRoleRequestDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleId { get; set; }

    }
}
