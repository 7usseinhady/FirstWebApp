using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.DTOs.Auth.Request
{
    public class UserRoleRequestDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleId { get; set; }

    }
}
