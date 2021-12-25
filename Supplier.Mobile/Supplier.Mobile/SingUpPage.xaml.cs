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
        private string _tankPlateNumber;
        public string TankPlateNumber
        {
            get { return _tankPlateNumber; }
            set { _tankPlateNumber = value; OnPropertyChanged(); }
        }
        private string _tankColor;
        public string TankColor
        {
            get { return _tankColor; }
            set { _tankColor = value; OnPropertyChanged(); }
        }
        private string _locationWork;
        public string LocationWork
        {
            get { return _locationWork; }
            set { _locationWork = value; OnPropertyChanged(); }
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
                var Uuid = Guid.NewGuid().ToString();
                var supplierProfile = new BLL.Models.Supplier
                {
                    id = Uuid,
                    full_name = FullName,
                    phone_number = PhoneNumber,
                    tank_size = TankSize,
                    estimated_price = EstmaiedPrice,
                    available = false,
                    tank_color = TankColor,
                    tank_plate_number = TankPlateNumber,
                    work_location = LocationWork
                };
                await BLL.Services.FirebaseService.AddNewSupplier(Uuid, supplierProfile);
                App.Current.MainPage = new NavigationPage(new MainPage(supplierProfile));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}