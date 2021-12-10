using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Notification
{
    public class PushTemplates
    {
        public class Generic
        {
            public const string Android = "{ \"notification\": { \"title\" : \"$(alertTitle)\", \"body\" : \"$(alertMessage)\"}, \"data\" : { \"title\" : \"$(alertTitle)\", \"message\" : \"$(alertMessage)\" } }";
            public const string iOS = "{ \"aps\" : {\"alert\" : \"$(alertMessage)\"} }";
        }

        public class Silent
        {
            public const string Android = "{ \"data\" : {\"title\" : \"$(alertTitle)\", \"message\" : \"$(alertMessage)\"} }";
            public const string iOS = "{ \"aps\" : {\"content-available\" : 1, \"apns-priority\": 5, \"sound\" : \"\", \"badge\" : 0}, \"message\" : \"$(alertMessage)\"}";
        }
    }
}
