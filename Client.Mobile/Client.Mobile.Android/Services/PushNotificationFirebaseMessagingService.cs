using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Client.Mobile.Interface;
using Client.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class PushNotificationFirebaseMessagingService : FirebaseMessagingService
    {
        IPushNotificationActionService _notificationActionService;
        INotificationRegistrationService _notificationRegistrationService;
        IDeviceInstallationService _deviceInstallationService;

        IPushNotificationActionService NotificationActionService
            => _notificationActionService ??=
                ServiceContainer.Resolve<IPushNotificationActionService>();

        INotificationRegistrationService NotificationRegistrationService
            => _notificationRegistrationService ??=
                ServiceContainer.Resolve<INotificationRegistrationService>();

        IDeviceInstallationService DeviceInstallationService
            => _deviceInstallationService ??=
                ServiceContainer.Resolve<IDeviceInstallationService>();

        public override void OnNewToken(string token)
        {
            DeviceInstallationService.Token = token;

            NotificationRegistrationService.RefreshRegistrationAsync()
                .ContinueWith((task) => { if (task.IsFaulted) throw task.Exception; });
        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            message.Data.TryGetValue("title", out var title);
            message.Data.TryGetValue("message", out var content);
            if(!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(content))
                NotificationActionService.TriggerAction(title,content);            
        }
    }
}