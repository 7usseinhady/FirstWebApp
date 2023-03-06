using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Runtime.ExceptionServices;
using WebApp.SharedKernel.Interfaces;

namespace WebApp.SharedKernel.Helpers.Notification.FCM
{
    public class NotificationService : INotificationService
    {
        private readonly FirebaseApp _firebaseApp;

        public NotificationService()
        {
            var path = @"wwwroot//FirebaseAuth.json";
            try
            {
                _firebaseApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                });
            }
            catch (Exception ex)
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
            }
        }

        public async Task<bool> SendNotificationToDeviceAsync(NotificationRequest notificationRequestModel)
        {
            try
            {
                notificationRequestModel.DataPayload.Add("title", notificationRequestModel.Title);
                notificationRequestModel.DataPayload.Add("body", notificationRequestModel.Body);
                var message = new Message()
                {
                    Notification = new FirebaseAdmin.Messaging.Notification() { Title = notificationRequestModel.Title, Body = notificationRequestModel.Body },
                    Data = notificationRequestModel.DataPayload,
                    Token = notificationRequestModel.DeviceId,
                };
                var response = await FirebaseMessaging.GetMessaging(_firebaseApp).SendAsync(message);
                return !string.IsNullOrEmpty(response);
            }

            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                return false;
            }
        }
        public async Task<bool> SendNotificationToTopicAsync(NotificationRequest notificationRequestModel)
        {
            try
            {
                notificationRequestModel.DataPayload.Add("title", notificationRequestModel.Title);
                notificationRequestModel.DataPayload.Add("body", notificationRequestModel.Body);
                var message = new Message()
                {
                    Notification = new FirebaseAdmin.Messaging.Notification() { Title = notificationRequestModel.Title, Body = notificationRequestModel.Body },
                    Data = notificationRequestModel.DataPayload,
                    Topic = notificationRequestModel.Topic,
                };
                var response = await FirebaseMessaging.GetMessaging(_firebaseApp).SendAsync(message);
                return !string.IsNullOrEmpty(response);
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                return false;
            }
        }

        public async Task<bool> SubscribeToTopicAsync(List<string> devicesTokens, string topic)
        {
            try
            {
                var response = await FirebaseMessaging.GetMessaging(_firebaseApp).SubscribeToTopicAsync(devicesTokens, topic);
                return response.SuccessCount == devicesTokens.Count;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                return false;
            }
        }
        public async Task<bool> UnsubscribeFromTopicAsync(List<string> devicesTokens, string topic)
        {
            try
            {
                var response = await FirebaseMessaging.GetMessaging(_firebaseApp).UnsubscribeFromTopicAsync(devicesTokens, topic);
                return response.SuccessCount == devicesTokens.Count;
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
                return false;
            }
        }


    }
}
