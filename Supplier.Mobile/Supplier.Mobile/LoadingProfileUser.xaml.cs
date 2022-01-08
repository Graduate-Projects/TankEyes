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
    public partial class LoadingProfileUser : ContentPage
    {
        public LoadingProfileUser(string PhoneNumber)
        {
            InitializeComponent();
            CheckProfileUser(PhoneNumber);
        }

        private async void CheckProfileUser(string PhoneNumber)
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