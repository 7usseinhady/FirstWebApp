using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Dtos.Auth.Request
{

    public class UserRegisterRequestDto : UserSharedRequestDto
    {
        [Display(Name = "UserName"), Required(ErrorMessage = "Required"), StringLength(50)]
        public string Username { get; set; } = default!;

        [Display(Name = "Password"), Required(ErrorMessage = "Required"), StringLength(256), RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[@$!%*?&#]).{6,}$", ErrorMessage = "PasswordRegularExpression")]
        public string Password { get; set; } = default!;

        [Display(Name = "ConfirmPassword"), Required(ErrorMessage = "Required"), StringLength(256), Compare("Password", ErrorMessage = "Not Match")]
        public string ConfirmPassword { get; set; } = default!;
        public bool IsBasedEmail { get; set; }
    }
}
