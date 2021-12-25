using Client.Mobile.Interface;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);

            Services.ServiceContainer.Resolve<IPushNotificationActionService>().MessageRecived += NotificationInApp;
            StartUpPage().ConfigureAwait(true);
        }

        private void NotificationInApp(object sender, BLL.Models.Notification.NotificationRequest Notification)
        {
            INotificationManager service = DependencyService.Get<INotificationManager>();
            service.SendNotification(Notification.Title, Notification.Text);
        }

        private async Task StartUpPage()
        {
            var IsFirstTime = await Xamarin.Essentials.SecureStorage.GetAsync("IsFirstTime");
            if (string.IsNullOrEmpty(IsFirstTime))
            {
                MainPage = new Walkthrough();
            }
            else
            {
                var ClientID = await Xamarin.Essentials.SecureStorage.GetAsync("ClientID");
#if DEBUG
                AppStatic.ClientID = "dc24bf22-4ba7-4be6-9b7e-261d12dc69a7";
                MainPage = new NavigationPage(new MainPage());
#else
                AppStatic.ClientID = ClientID;
                if (string.IsNullOrEmpty(ClientID))
                    MainPage = new NavigationPage(new QRScanner());
                else
                    MainPage = new NavigationPage(new MainPage());
#endif
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
