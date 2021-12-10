using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Mobile.Interface
{
    public interface INotificationActionService
    {
        void TriggerAction(string title,string message);
    }
}
