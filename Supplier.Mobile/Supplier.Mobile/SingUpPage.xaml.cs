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
    public partial class SingUpPage : ContentPage
    {
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }
        private string _estmaiedPrice;
        public string EstmaiedPrice
        {
            get { return _estmaiedPrice; }
            set { _estmaiedPrice = value; OnPropertyChanged(); }
        }
        private string _tankSize;
        public string TankSize
        {
            get { return _tankSize; }
            set { _tankSize = value; OnPropertyChanged(); }
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        public SingUpPage(string phone_number)
        {
            InitializeComponent();
            this.BindingContext = this;
            PhoneNumber = phone_number;
        }

        private async void SingUpClicked(object sender, EventArgs e)
        {
            try
            {
                var Uuid = Guid.NewGuid();
                var supplierProfile = new BLL.Models.Supplier
                {
                    id = Uuid.ToString(),
                    full_name = FullName,
                    phone_number = PhoneNumber,
                    tank_size = TankSize,
                    estimated_price = EstmaiedPrice,
                    available = false
                };
                await BLL.Services.FirebaseService.AddNewSupplier(supplierProfile);
                App.Current.MainPage = new NavigationPage(new MainPage(supplierProfile));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}