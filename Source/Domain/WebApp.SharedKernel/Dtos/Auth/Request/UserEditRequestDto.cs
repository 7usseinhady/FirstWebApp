using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Dtos.Auth.Request
{
    public class UserEditRequestDto : UserSharedRequestDto
    {
        [Required]
        public string Id { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

    }
}