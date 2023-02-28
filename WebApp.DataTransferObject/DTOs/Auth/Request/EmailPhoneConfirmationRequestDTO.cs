using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.DTOs.Auth.Request
{
    public class EmailPhoneConfirmationRequestDTO
    {
        [Display(Name = "PersonalKey"), Required(ErrorMessage = "Required")]
        public string PersonalKey { get; set; }

        [Display(Name = "TokenCode"), Required(ErrorMessage = "Required"), StringLength(6)]
        public string TokenCode { get; set; }
    }
}
