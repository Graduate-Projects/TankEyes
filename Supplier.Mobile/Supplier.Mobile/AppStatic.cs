using BLL.Extensions;
using Supplier.Mobile.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Supplier.Mobile
{
    public static class AppStatic
    {
        public static string PhoneNumber { get; set; }
        public static string SupplierID { get; set; }

        public static async Task RegisterationDevices()
        {
            try
            {
                var tags = new List<string>();
                tags.Add("tank-eyes-suppliers".Validation());
                tags.Add(SupplierID.Validation());

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
