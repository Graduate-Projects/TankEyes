using System;
using System.Collections.Generic;
using System.Text;

namespace Supplier.Mobile.Interface
{
    public interface INotificationActionService
    {
        void TriggerAction(string title,string message);
    }
}
