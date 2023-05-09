using System.ComponentModel.DataAnnotations;

namespace WebApp.SharedKernel.Dtos.Auth.Request
{
    public class PersonalKeyRequestDto
    {
        [Display(Name = "PersonalKey"), Required(ErrorMessage = "Required")]
        public string PersonalKey { get; set; } = default!;


    }
}
