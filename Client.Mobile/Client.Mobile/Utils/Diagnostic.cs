using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Mobile.Utils
{
    public static class Diagnostic
    {
        public static void Log(Exception Exception, string Message = null)
        {
            IDictionary<string, string> propreties = null;
            if (!string.IsNullOrEmpty(Message)) propreties = new Dictionary<string, string>() { { "Message", Message } };
            Crashes.TrackError(Exception, propreties);
        }
        public static void LogEvent(string EventName, Dictionary<string, string> Message)
        {
            Analytics.TrackEvent(EventName, Message);
        }
    }
}
