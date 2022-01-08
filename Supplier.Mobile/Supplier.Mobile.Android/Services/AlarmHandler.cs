using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Supplier.Mobile.Droid;


namespace Supplier.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class AlarmHandler : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent?.Extras != null)
            {
                PowerManager.WakeLock sWakeLock;
                var pm = PowerManager.FromContext(context);
                sWakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "GCM Broadcast Reciever Tag");
                sWakeLock.Acquire();

                string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
                string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);

                // Instantiate the builder and set notification elements:
                NotificationCompat.Builder builder = new NotificationCompat.Builder(MainActivity.Instance, "default.supplier.tank-eyes")
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetSmallIcon(Resource.Drawable.ic_notify);

                // Build the notification:
                Notification notification = builder.Build();

                // Get the notification manager:
                var notificationManager = NotificationManagerCompat.From(MainActivity.Instance);
                // Publish the notification:
                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);
            }
        }
    }
}