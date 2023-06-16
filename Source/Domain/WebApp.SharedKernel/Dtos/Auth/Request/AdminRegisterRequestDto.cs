namespace WebApp.SharedKernel.Dtos.Auth.Request
{
    public class AdminRegisterRequestDto : UserRegisterRequestDto
    {
        public string RoleName { get; set; } = default!;
    }
}
