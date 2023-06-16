
namespace WebApp.Core.Helpers.Sms.GatewaySms
{
    public class GatewaySmsConfiguration
    {
        public string api_id { get; set; } = default!;
        public string api_password { get; set; } = default!;
        public string sender_id { get; set; } = default!;
        public string? templateid { get; set; } = default!;
    }
}
