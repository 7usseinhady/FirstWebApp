using System.ComponentModel.DataAnnotations;
using WebApp.SharedKernel.Interfaces;

namespace WebApp.SharedKernel.Dtos.Auth.Request
{
    public class UserSharedRequestDto : IInactive
    {
        [Display(Name = "FirstName"), Required(ErrorMessage = "Required"), MaxLength(100)]
        public string FirstName { get; set; } = default!;

        [Display(Name = "LastName"), Required(ErrorMessage = "Required")]
        public string LastName { get; set; } = default!;


        [Display(Name = "Email"), EmailAddress(ErrorMessage = "Invalid")]
        public string? Email { get; set; } = default!;
        public bool? IsFemale { get; set; }

        public string? NationalPhoneNumber { get; set; }

        [MaxLength(13)]
        public string? PhoneNumber { get; set; }

        public string? NationalPhoneNumber2 { get; set; }

        [MaxLength(13)]
        public string? PhoneNumber2 { get; set; }

        [Display(Name = "Inactive")]
        public bool IsInactive { get; set; }
    }
}
