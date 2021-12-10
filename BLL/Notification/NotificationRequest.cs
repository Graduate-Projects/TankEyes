using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Notification
{
    public class NotificationRequest
    {
        public string Text { get; set; }
        //public string Action { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public bool Silent { get; set; }
        public string Title { get; set; }
    }
}
