namespace WebApp.SharedKernel.Dtos.Auth.Request
{
    public class UserLangRequestDto : UserIdRequestDto
    {
        public string LastLang { get; set; } = default!;
    }
}
