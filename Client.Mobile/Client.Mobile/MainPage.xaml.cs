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
            timer = new Timer();
            timer.Interval = 100;
            timer.Elapsed += (s, e) => LoadConfigration().ConfigureAwait(false);
            timer.Start();
        }
        public async Task LoadConfigration()
        {
            Client = await BLL.Services.FirebaseService.UpdateInfoTank(Guid.Parse("dc24bf22-4ba7-4be6-9b7e-261d12dc69a7"));
            OnPropertyChanged(nameof(MotorToggled));
        }

        private void OpenSupplierList(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SupplierList());
        }

        private async void TriggerPumb(object sender, EventArgs e)
        {
            Client = await BLL.Services.FirebaseService.TriggerPumb(Guid.Parse("dc24bf22-4ba7-4be6-9b7e-261d12dc69a7"));
        }

        private async void SwitchChanged(bool Value)
        {
            try
            {
                var IsAllowedToRunMotorAsAuto = Value;
                Client = await BLL.Services.FirebaseService.ChangeConfigPumb(Guid.Parse("dc24bf22-4ba7-4be6-9b7e-261d12dc69a7"), IsAllowedToRunMotorAsAuto);
            }
            catch (Exception)
            {
                SwitchMotorAuto.IsToggled = !Value;
            }
        }
    }
}
