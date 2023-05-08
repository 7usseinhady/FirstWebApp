using WebApp.SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Dtos.Auth.Request
{
    public class UserRequestDto : IInactive
    {
        [Required]
        public string Id { get; set; } = default!;

        [Display(Name = "FirstName"), Required(ErrorMessage = "Required"), MaxLength(100)]
        public string FirstName { get; set; } = default!;

        [Display(Name = "LastName"), Required(ErrorMessage = "Required")]
        public string LastName { get; set; } = default!;


        [Display(Name = "Email"), EmailAddress(ErrorMessage = "Invalid")]
        public string? Email { get; set; } = default!;

        [Required, MaxLength(4)]
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
                return Code + subNumber;
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
                return Code + subNumber;
            }
        }

        public bool? IsFemale { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Inactive")]
        public bool IsInactive { get; set; }
    }
}