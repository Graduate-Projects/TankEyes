using MediaManager;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Supplier.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent(); 
            CrossMediaManager.Current.Init();
            XF.Material.Forms.Material.Init(this);
            //InitSounds();
            StartUpPage().ConfigureAwait(true);
        }

        private void InitSounds()
        {
            var cacheFile = Path.Combine(FileSystem.CacheDirectory, "notification.mp3");
            if (File.Exists(cacheFile))
                File.Delete(cacheFile);
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("notification.mp3"))
            using (var file = new FileStream(cacheFile, FileMode.Create, FileAccess.Write))
            {
                resource.CopyTo(file);
            }
        }

        private async Task StartUpPage()
        {
            var PhoneNumber = await Xamarin.Essentials.SecureStorage.GetAsync("PhoneNumber");
#if DEBUG
            PhoneNumber = "+962785461900";
#endif
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                MainPage = new NavigationPage(new SignInPage());
            }
            else
            {
                MainPage = new LoadingProfileUser(PhoneNumber);
            }
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
