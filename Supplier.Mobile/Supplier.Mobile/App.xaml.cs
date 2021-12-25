using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Supplier.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);

            StartUpPage().ConfigureAwait(true);
        }
        private async Task StartUpPage()
        {
            var PhoneNumber = await Xamarin.Essentials.SecureStorage.GetAsync("PhoneNumber");
#if DEBUG
            AppStatic.PhoneNumber = "+962785461900";
            var suppliers = await BLL.Services.FirebaseService.GetAllSuppliersAsync();
            var supplierProfile = suppliers.FirstOrDefault(spp => spp.phone_number == AppStatic.PhoneNumber);
            if (supplierProfile == null)
            {
                App.Current.MainPage = new SingUpPage(AppStatic.PhoneNumber);
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new MainPage(supplierProfile));
            }
#else
            AppStatic.PhoneNumber = PhoneNumber;
            if (string.IsNullOrEmpty(PhoneNumber))
                MainPage = new NavigationPage(new SignInPage());
            else
            {            
                var suppliers = await BLL.Services.FirebaseService.GetAllSuppliersAsync();
                var supplierProfile = suppliers.FirstOrDefault(spp => spp.phone_number == AppStatic.PhoneNumber);
                if (supplierProfile == null)
                {
                    App.Current.MainPage = new SingUpPage(AppStatic.PhoneNumber);
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(new MainPage(supplierProfile));
                }
            }
#endif
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
