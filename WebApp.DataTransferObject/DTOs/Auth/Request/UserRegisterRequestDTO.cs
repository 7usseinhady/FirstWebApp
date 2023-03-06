using WebApp.DataTransferObjects.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.Dtos.Auth.Request
{

    public class UserRegisterRequestDto
    {

        [Display(Name = "FirstName"), Required(ErrorMessage = "Required"), MaxLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "LastName"), Required(ErrorMessage = "Required"), MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "UserName"), Required(ErrorMessage = "Required"), StringLength(50)]
        public string Username { get; set; }

        [Display(Name = "Email"), EmailAddress(ErrorMessage = "Invalid"), StringLength(128)]
        public string? Email { get; set; }

        public bool? IsFemale { get; set; }

        public string Code { get; set; } = "+966";

        [Display(Name = "PhoneNumber"),
            MinLength(10, ErrorMessage = "Invalid"),
            MaxLength(10, ErrorMessage = "Invalid"),
            RegularExpression("^05[0-9]{8}$", ErrorMessage = "PhoneRegularExpression")]
        public string? LocalPhoneNumber { get; set; }

        [MaxLength(13)]
        public string? PhoneNumber
        {
            get
            {
                var subNumber = (LocalPhoneNumber ?? "0").StartsWith("0") ? LocalPhoneNumber?.Substring(1) : LocalPhoneNumber;
                if (!string.IsNullOrEmpty(subNumber))
                    return Code + subNumber;
                else
                    return null;
            }
        }

        [Display(Name = "SecondPhoneNumber"),
            MinLength(10, ErrorMessage = "Invalid"),
            MaxLength(10, ErrorMessage = "Invalid"),
            RegularExpression("^05[0-9]{8}$", ErrorMessage = "PhoneRegularExpression")]
        public string? SecondLocalPhoneNumber { get; set; }

        [MaxLength(13)]
        public string? SecondPhoneNumber
        {
            get
            {
                var subNumber = (SecondLocalPhoneNumber ?? "0").StartsWith("0") ? SecondLocalPhoneNumber?.Substring(1) : SecondLocalPhoneNumber;
                if (!string.IsNullOrEmpty(subNumber))
                    return Code + subNumber;
                else
                    return null;
            }
        }


        [Display(Name = "Password"), Required(ErrorMessage = "Required"), StringLength(256), RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[@$!%*?&#]).{6,}$", ErrorMessage = "PasswordRegularExpression")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword"), Required(ErrorMessage = "Required"), StringLength(256), Compare("Password", ErrorMessage = "Not Match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Inactive")]
        public bool IsInactive { get; set; }

        public bool IsBasedEmail { get; set; }
    }
}
