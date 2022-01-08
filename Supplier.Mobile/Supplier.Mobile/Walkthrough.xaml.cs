using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Supplier.Mobile
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
            var status_location = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();
            if (status_location != Xamarin.Essentials.PermissionStatus.Granted)
            {
                await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationWhenInUse>();
            }

            var PhoneNumber = await Xamarin.Essentials.SecureStorage.GetAsync("PhoneNumber");
#if DEBUG
            PhoneNumber = "+962785461900";
#endif
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                App.Current.MainPage = new NavigationPage(new SignInPage());
            }
            else
            {
                AppStatic.PhoneNumber = PhoneNumber;
                var suppliers = await BLL.Services.FirebaseService.GetAllSuppliersAsync().ConfigureAwait(true);
                var supplierProfile = suppliers?.FirstOrDefault(spp => spp.phone_number == PhoneNumber);
                if (supplierProfile == null)
                {
                    App.Current.MainPage = new SingUpPage(PhoneNumber);
                }
                else
                {
                    AppStatic.SupplierID = supplierProfile.id;
                    App.Current.MainPage = new NavigationPage(new MainPage(supplierProfile));
                }
            }
        }
    }
}