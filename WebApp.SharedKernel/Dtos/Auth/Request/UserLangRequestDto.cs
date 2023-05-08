namespace WebApp.SharedKernel.Dtos.Auth.Request
{
    public class UserLangRequestDto
    {
        public string UserId { get; set; } = default!;
        public string LastLang { get; set; } = default!;
    }
}
