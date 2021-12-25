using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Walkthrough : CarouselPage
    {
        public Walkthrough()
        {
            InitializeComponent();
        }

        private void SkipMove(object sender, EventArgs e)
        {
            this.CurrentPage = this.Children.Last();
        }

        private void NextMove(object sender, EventArgs e)
        {
            var indexCurrentPageSelected = GetIndex(this.CurrentPage);
            this.CurrentPage = GetPageByIndex(indexCurrentPageSelected + 1);
        }

        private async void ClosePage(object sender, EventArgs e)
        {
            var status = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.Camera>();
            if (status != Xamarin.Essentials.PermissionStatus.Granted)
            {
                Task.WaitAll(new Task[]{
                    Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Camera>(),
                    Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Flashlight>()
                });
            }

            var ClientID = await Xamarin.Essentials.SecureStorage.GetAsync("ClientID");
#if DEBUG
            AppStatic.ClientID = "dc24bf22-4ba7-4be6-9b7e-261d12dc69a7";
            App.Current.MainPage = new NavigationPage(new MainPage());
#else
            await Xamarin.Essentials.SecureStorage.SetAsync("IsFirstTime", "False");
            AppStatic.ClientID = ClientID;
            if (string.IsNullOrEmpty(ClientID))
                App.Current.MainPage = new NavigationPage(new QRScanner());
            else
                App.Current.MainPage = new NavigationPage(new MainPage());
#endif
        }
    }
}