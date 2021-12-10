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

namespace Supplier.Mobile.Droid
{
    [Activity(Label = "Supplier Tanks", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            CrossFirebase.Initialize(this, savedInstanceState, CreateCrossFirebaseSettings());
            FirebaseApp.InitializeApp(Application.Context);
            LoadApplication(new App());
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

    }
}