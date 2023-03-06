
namespace WebApp.SharedKernel.Helpers.SMS.GatewaySMS
{
    public class GatewaySMSRequest : GatewaySMSConfiguration
    {
        public string phonenumber { get; set; }
        public string? textmessage { get; set; }
        
        public string sms_type { get; set; } = "T";
        public string encoding { get; set; } = "T";

        public string? V1 { get; set; }
        public string? V2 { get; set; }
        public string? V3 { get; set; }
        public string? V4 { get; set; }
        public string? V5 { get; set; }
        public int? ValidityPeriodInSeconds { get; set; }
        // uid and  callback_url  parameters is not supported with SendSMSMulti  API. Only for SendSMS API.
        public string? uid { get; set; }
        public string? callback_url { get; set; }
        public string? pe_id { get; set; }
        public string? template_id { get; set; }
    }
}
