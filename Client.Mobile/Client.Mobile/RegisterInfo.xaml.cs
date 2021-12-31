using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<string> Regions { get; set; }

        private string _RegionSelected;
        public string RegionSelected
        {
            get { return _RegionSelected; }
            set { _RegionSelected = value; OnPropertyChanged(); }
        }

        public RegisterInfo()
        {
            InitializeComponent();
            this.BindingContext = this;
            Regions = new ObservableCollection<string>();
            Initialization();
        }
        private async void Initialization()
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
                var client = new BLL.Models.Client
                {
                    full_name = FullName,
                    phone_number = PhoneNumber,
                    tank_size = TankSize,
                    home_location = RegionSelected
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