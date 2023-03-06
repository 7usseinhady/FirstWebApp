using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.Dtos.Auth.Request
{
    public class UserLoginRequestDto
    {
        [Display(Name = "PersonalKey"), Required(ErrorMessage = "Required"), MaxLength(100)]
        public string PersonalKey { get; set; } = default!;

        [Display(Name = "Password"), Required(ErrorMessage = "Required")]
        public string Password { get; set; } = default!;
    }
}
