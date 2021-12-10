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
    public partial class RegisterInfo : ContentPage
    {
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
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

        public RegisterInfo()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        private async void SingUpClicked(object sender, EventArgs e)
        {
            try
            {
                var client = new BLL.Models.Client
                {
                    full_name = FullName,
                    phone_number = PhoneNumber,
                    tank_size = TankSize,
                    address = Address
                };
                await BLL.Services.FirebaseService.AddNewClient(AppStatic.ClientID, client);
                App.Current.MainPage = new NavigationPage(new MainPage());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}