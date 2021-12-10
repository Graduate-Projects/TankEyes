using Client.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Client.Mobile.Interface;

namespace Client.Mobile.Utils
{
    public static class Bootstrap
    {
        public static void Begin(Func<IDeviceInstallationService> deviceInstallationService)
        {
            ServiceContainer.Register(deviceInstallationService);

            ServiceContainer.Register<IPushNotificationActionService>(()
                => new PushNotificationActionService());

            ServiceContainer.Register<INotificationRegistrationService>(()
                => new NotificationRegistrationService($"{BLL.Settings.Configration.ApiServerAddress}/"));
        }
    }
}
