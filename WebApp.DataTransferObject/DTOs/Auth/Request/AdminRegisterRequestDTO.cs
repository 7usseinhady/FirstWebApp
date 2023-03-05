namespace WebApp.DataTransferObjects.DTOs.Auth.Request
{
    public class AdminRegisterRequestDTO : UserRegisterRequestDTO
    {
        public string RoleName { get; set; }
    }
}
