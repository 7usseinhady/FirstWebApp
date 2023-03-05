using WebApp.SharedKernel.Helpers.Notification.FCM;

namespace WebApp.SharedKernel.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendNotificationToDeviceAsync(NotificationRequest notificationRequestModel);
        Task<bool> SendNotificationToTopicAsync(NotificationRequest notificationRequestModel);

        Task<bool> SubscribeToTopicAsync(List<string> devicesTokens, string topic);
        Task<bool> UnsubscribeFromTopicAsync(List<string> devicesTokens, string topic);

        

    }
}
