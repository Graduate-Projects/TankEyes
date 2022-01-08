using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.Firebase.CloudMessaging;
using Android.Content;
using Plugin.Firebase.DynamicLinks;
using Plugin.Firebase.Shared;
using Plugin.Firebase.Android;
using Firebase;
using Supplier.Mobile.Interface;
using Supplier.Droid.Services;
using Android.Gms.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;

namespace Supplier.Mobile.Droid
{
    [Activity(Label = "TANK SUPPLIER", Icon = "@mipmap/ic_launcher", Theme = "@style/SplashScreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IOnCompleteListener, IApplication
    {
        public static Context Instance { get; internal set; }
        private IPushNotificationActionService _notificationActionService;
        private IDeviceInstallationService _deviceInstallationService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            CrossFirebase.Initialize(this, savedInstanceState, CreateCrossFirebaseSettings());
            FirebaseApp.InitializeApp(Application.Context);
            Utils.Bootstrap.Begin(() => new DeviceInstallationService());
            if (DeviceInstallationService.NotificationsSupported)
            {
                Firebase.Messaging.FirebaseMessaging.Instance
                    .GetToken().AddOnCompleteListener(this);
            }
            AppCenter.Start("995c143a-4a6b-4bd6-849a-7009ede85a78", typeof(Analytics), typeof(Crashes));

            Instance = this;

            LoadApplication(new App());
            ProcessNotificationActions(Intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(
                isAnalyticsEnabled: true,
                isAuthEnabled: true,
                isCloudMessagingEnabled: false,
                isDynamicLinksEnabled: false,
                isFirestoreEnabled: false,
                isFunctionsEnabled: false,
                isRemoteConfigEnabled: false,
                isStorageEnabled: false
                //facebookId: "151743924915235",
                //facebookAppName: "Plugin Firebase Playground",
                //googleRequestIdToken: "537235599720-723cgj10dtm47b4ilvuodtp206g0q0fg.apps.googleusercontent.com"
                );
        }
        public void OnComplete(Android.Gms.Tasks.Task token)
        {
            if (token.IsSuccessful)
            {
                DeviceInstallationService.Token = token.Class.GetMethod("getResult").Invoke(token).ToString();
                AppStatic.RegisterationDevices().ConfigureAwait(false);
            }
            else
            {
                //Utils.Diagnostic.Log(token.Exception, "can not register token from firebase");
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