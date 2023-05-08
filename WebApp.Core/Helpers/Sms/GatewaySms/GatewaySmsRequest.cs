
namespace WebApp.Core.Helpers.Sms.GatewaySms
{
    public class GatewaySmsRequest : GatewaySmsConfiguration
    {
        public string phonenumber { get; set; } = default!;
        public string? textmessage { get; set; } = default!;

        public string sms_type { get; set; } = "T";
        public string encoding { get; set; } = "T";

        public string? V1 { get; set; } = default!;
        public string? V2 { get; set; } = default!;
        public string? V3 { get; set; } = default!;
        public string? V4 { get; set; } = default!;
        public string? V5 { get; set; } = default!;
        public int? ValidityPeriodInSeconds { get; set; }
        // uid and  callback_url  parameters is not supported with SendSmsMulti  API. Only for SendSms API.
        public string? uid { get; set; }
        public string? callback_url { get; set; }
        public string? pe_id { get; set; }
        public string? template_id { get; set; }
    }
}
