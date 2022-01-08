using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Supplier.Mobile.Interface
{
    public interface INotificationRegistrationService
    {
        Task UnRegisterDeviceAsync();
        Task RegisterDeviceAsync(params string[] tags);
        Task RefreshRegistrationAsync();
    }
}
