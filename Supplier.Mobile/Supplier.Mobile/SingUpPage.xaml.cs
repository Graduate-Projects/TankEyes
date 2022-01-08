using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<string> Regions { get; set; }

        private string _RegionSelected;
        public string RegionSelected
        {
            get { return _RegionSelected; }
            set { _RegionSelected = value; OnPropertyChanged(); }
        }


        public SingUpPage(string phone_number)
        {
            InitializeComponent();
            Regions = new ObservableCollection<string>();
            PhoneNumber = phone_number;
            this.BindingContext = this;
            Initialization().ConfigureAwait(false);
        }
        private async Task Initialization()
        {
            try
            {
                using (await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance.LoadingDialogAsync($"Get Regions...", Configration.MaterialConfigration.LoadingDialogConfiguration))
                {
                    var regions = await BLL.Services.FirebaseService.GetRegionsAsync();
                    if (regions != null) Regions = new ObservableCollection<string>(regions);
                    OnPropertyChanged(nameof(Regions));
                }
            }
            catch (Exception ex)
            {
                //Utils.Diagnostic.Log(ex);
            }
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
                    work_location = RegionSelected
                };
                await BLL.Services.FirebaseService.AddNewSupplier(Uuid, supplierProfile);
                AppStatic.PhoneNumber = PhoneNumber;
                AppStatic.SupplierID = Uuid;
                App.Current.MainPage = new NavigationPage(new MainPage(supplierProfile));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}