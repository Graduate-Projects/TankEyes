using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Client.Mobile.Interface;
using Android.Content;
using Android.Gms.Tasks;
using Client.Droid.Services;

namespace Client.Mobile.Droid
{
    [Activity(Label = "Tank Eyes", Icon = "@mipmap/ic_launcher", 
        Theme = "@style/MainTheme", 
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,IOnCompleteListener, IApplication
    {
        public static Context Instance { get; internal set; }
        private IPushNotificationActionService _notificationActionService;
        private IDeviceInstallationService _deviceInstallationService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState); 
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            Utils.Bootstrap.Begin(() => new DeviceInstallationService());
            if (DeviceInstallationService.NotificationsSupported)
            {
                Firebase.Messaging.FirebaseMessaging.Instance
                    .GetToken().AddOnCompleteListener(this);
            }
            AppCenter.Start("52f2e43b-6169-4062-b0fe-7b205e28dc1a",typeof(Analytics), typeof(Crashes));
            Instance = this;
            LoadApplication(new App());
            ProcessNotificationActions(Intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void OnComplete(Task token)
        {
            if (token.IsSuccessful)
            {
                DeviceInstallationService.Token = token.Class.GetMethod("getResult").Invoke(token).ToString();
                AppStatic.RegisterationDevices().ConfigureAwait(false);
            }
            else
            {
                Utils.Diagnostic.Log(token.Exception, "can not register token from firebase");
            }
        }

        IPushNotificationActionService NotificationActionService
            => _notificationActionService ??= Services.ServiceContainer.Resolve<IPushNotificationActionService>();

        IDeviceInstallationService DeviceInstallationService
            => _deviceInstallationService ??= Services.ServiceContainer.Resolve<IDeviceInstallationService>();


        void ProcessNotificationActions(Intent intent)
        {
            try
            {
                string title = intent.GetStringExtra("title");
                string content = intent.GetStringExtra("message");
                if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(content))
                    NotificationActionService.TriggerAction(title, content);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            ProcessNotificationActions(intent);
        }

    }
}