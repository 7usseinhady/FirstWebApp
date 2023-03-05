
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.DTOs.Auth.Request
{
    public class RoleRequestDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
