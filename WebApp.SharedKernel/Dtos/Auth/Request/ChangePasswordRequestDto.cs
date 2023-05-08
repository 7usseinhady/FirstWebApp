using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Dtos.Auth.Request
{
    public class ChangePasswordRequestDto
    {
        public string? RefreshToken { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "OldPassword"), Required(ErrorMessage = "Required"), StringLength(256)]
        public string OldPassword { get; set; } = default!;

        [DataType(DataType.Password)]
        [Display(Name = "NewPassword"), Required(ErrorMessage = "Required"), StringLength(256), RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[@$!%*?&#]).{6,}$", ErrorMessage = "PasswordRegularExpression")]

        public string NewPassword { get; set; } = default!;

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword"), Required(ErrorMessage = "Required"), StringLength(256), Compare("NewPassword", ErrorMessage = "Not Match")]
        public string ConfirmNewPassword { get; set; } = default!;
    }
}
