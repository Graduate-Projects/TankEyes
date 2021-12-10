using BLL.Extensions;
using Client.Mobile.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Mobile
{
    public static class AppStatic
    {
        public static string ClientID { get; set; }

        public static async Task RegisterationDevices()
        {
            try
            {
                var tags = new List<string>();
                tags.Add("tank-eyes-clients".Validation());
                tags.Add(ClientID.Validation());

                var _notificationRegistrationService = Services.ServiceContainer.Resolve<INotificationRegistrationService>();
                await _notificationRegistrationService.RegisterDeviceAsync(tags.ToArray());
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex);
            }
        }
    }
}
