using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;

namespace Client.Mobile
{
    public partial class MainPage : ContentPage
    {
        private BLL.Models.Client _Client;
        public BLL.Models.Client Client
        {
            get { return _Client; }
            set { _Client = value; OnPropertyChanged(); }
        }
        public bool MotorToggled
        {
            get { return Client?.auto??false; }
            set { if (value != Client.auto) { SwitchChanged(value); OnPropertyChanged(); } }
        }

        public Timer timer { get; set; }
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;

            LoadConfigration().ConfigureAwait(false);
            timer = new Timer
            {
                Interval = 100
            };
            timer.Elapsed += (s, e) => LoadConfigration().ConfigureAwait(false);
            timer.Start();
        }
        public async Task LoadConfigration()
        {
            Client = await BLL.Services.FirebaseService.GetClient(AppStatic.ClientID);
            OnPropertyChanged(nameof(MotorToggled));
        }

        private void OpenSupplierList(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SupplierList());
        }

        private async void TriggerPumb(object sender, EventArgs e)
        {
            Client.pumb_status = !Client.pumb_status;
            await BLL.Services.FirebaseService.UpdateClient(AppStatic.ClientID, Client);
            OnPropertyChanged(nameof(Client));
        }

        private async void SwitchChanged(bool Value)
        {
            try
            {
                Client.auto = Value;
                await BLL.Services.FirebaseService.UpdateClient(AppStatic.ClientID, Client);
                OnPropertyChanged(nameof(Client));
            }
            catch (Exception)
            {
                SwitchMotorAuto.IsToggled = !Value;
            }
        }
    }
}
