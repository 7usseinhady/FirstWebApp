
namespace WebApp.SharedKernel.Helpers.SMS.GatewaySMS
{
    public class GatewaySMSConfiguration
    {
        public string api_id { get; set; }
        public string api_password { get; set; }
        public string sender_id { get; set; }
        public string? templateid { get; set; }
    }
}
