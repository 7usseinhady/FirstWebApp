using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.DTOs.Auth.Request
{
    public class UserLoginRequestDTO
    {
        [Display(Name = "PersonalKey"), Required(ErrorMessage = "Required"), MaxLength(100)]
        public string PersonalKey { get; set; }

        [Display(Name = "Password"), Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
}
