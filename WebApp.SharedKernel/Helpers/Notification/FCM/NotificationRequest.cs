using Newtonsoft.Json;

namespace WebApp.SharedKernel.Helpers.Notification.FCM
{
    public class NotificationRequest
    {
        public NotificationRequest()
        {
            DataPayload = new Dictionary<string, string>();
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        public string? DeviceId { get; set; }
        public string? Topic { get; set; }

        public Dictionary<string, string> DataPayload { get; set; }


    }
}
