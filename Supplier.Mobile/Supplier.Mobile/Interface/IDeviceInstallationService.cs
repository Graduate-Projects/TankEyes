using Supplier.Mobile.Models.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supplier.Mobile.Interface
{
    public interface IDeviceInstallationService
    {
        string Token { get; set; }
        bool NotificationsSupported { get; }
        string GetDeviceId();
        DeviceInstallation GetDeviceInstallation(params string[] tags);
    }
}
