namespace WebApp.DataTransferObject.Dtos.Auth.Request
{
    public class UserDeviceIdRequestDto : UserIdRequestDto
    {
        public string DeviceId { get; set; } = default!;
    }
}
