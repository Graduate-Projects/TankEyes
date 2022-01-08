using System;
using System.Collections.Generic;
using System.Text;

namespace Supplier.Mobile.Interface
{
    public interface IPushNotificationActionService : INotificationActionService
    {
        event EventHandler<BLL.Models.Notification.NotificationRequest> MessageRecived;
    }
}
