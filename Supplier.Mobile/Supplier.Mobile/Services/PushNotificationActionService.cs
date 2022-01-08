using BLL.Enums;
using Supplier.Mobile.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Supplier.Mobile.Services
{
    public class PushNotificationActionService : IPushNotificationActionService
    {
        public event EventHandler<BLL.Models.Notification.NotificationRequest> MessageRecived = delegate { };

        public void TriggerAction(string title,string message)
        {
            List<Exception> exceptions = new List<Exception>();

            foreach (var handler in MessageRecived?.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(this, new BLL.Models.Notification.NotificationRequest
                    {
                        Title = title,
                        Text = message
                    });
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }
    }
}
