using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObject.Dtos.Auth.Request
{
    public class ResetPasswordRequestDto
    {
        [Required]
        public string personalKey { get; set; } = default!;

        [Display(Name = "TokenCode"), Required(ErrorMessage = "Required")]
        public string TokenCode { get; set; } = default!;

        [DataType(DataType.Password)]
        [Display(Name = "Password"), Required(ErrorMessage = "Required"), StringLength(256), RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[@$!%*?&#]).{6,}$", ErrorMessage = "PasswordRegularExpression")]

        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword"), Required(ErrorMessage = "Required"), StringLength(256), Compare("Password", ErrorMessage = "Not Match")]
        public string ConfirmPassword { get; set; } = default!;


    }
}
