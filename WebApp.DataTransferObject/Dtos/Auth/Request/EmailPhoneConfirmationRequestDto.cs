using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObject.Dtos.Auth.Request
{
    public class EmailPhoneConfirmationRequestDto
    {
        [Display(Name = "PersonalKey"), Required(ErrorMessage = "Required")]
        public string PersonalKey { get; set; } = default!;

        [Display(Name = "TokenCode"), Required(ErrorMessage = "Required"), StringLength(6)]
        public string TokenCode { get; set; } = default!;
    }
}
