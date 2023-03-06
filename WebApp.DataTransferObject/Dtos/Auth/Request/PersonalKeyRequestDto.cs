using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObjects.Dtos.Auth.Request
{
    public class PersonalKeyRequestDto
    {
        [Display(Name = "PersonalKey"), Required(ErrorMessage = "Required")]
        public string PersonalKey { get; set; }


    }
}
