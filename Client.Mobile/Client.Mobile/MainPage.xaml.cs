using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
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

        public Timer TimerGetUpdateProfile { get; set; }
        public Timer TimerUpdateLocation { get; set; }
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;

            LoadConfigration().ConfigureAwait(false);

            TimerGetUpdateProfile = new System.Timers.Timer();
            TimerGetUpdateProfile.Interval = TimeSpan.FromMilliseconds(100).TotalMilliseconds;
            TimerGetUpdateProfile.Elapsed += (s, e) => LoadConfigration().ConfigureAwait(false);
            TimerGetUpdateProfile.Start();

            TimerUpdateLocation = new System.Timers.Timer();
            TimerUpdateLocation.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
            TimerUpdateLocation.Elapsed += (s, e) => UpdateCurrentLocation().ConfigureAwait(false);
            TimerUpdateLocation.Start();
        }
        public async Task LoadConfigration()
        {
            Client = await BLL.Services.FirebaseService.GetClient(AppStatic.ClientID);
            if(Client.Notification == 1)
            {
                SendNotification("Water level", "The water level inside your tank is too low please fill up your tank to avoid the pump from burning out");
                Client.Notification = 0;
                await BLL.Services.FirebaseService.UpdateClient(Client.id, Client).ConfigureAwait(false);
            }
            OnPropertyChanged(nameof(MotorToggled));
        }
        public async Task UpdateCurrentLocation()
        {
            var CurentLocation = await Utils.Location.GetCurrentLocation(new System.Threading.CancellationTokenSource());
            Client.latitude = CurentLocation.Latitude;
            Client.longitude = CurentLocation.Longitude;
            await BLL.Services.FirebaseService.UpdateClient(Client.id, Client).ConfigureAwait(false);
        }


        private void OpenSupplierList(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SupplierList(Client));
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

        private async void SendNotification(string title, string message)
        {
            try
            {
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://tank-eyes.azurewebsites.net/api/Notifications/requests", new BLL.Models.Notification.NotificationRequest
                    {
                        Title = title,
                        Text = message,
                        Tags = new string[] { BLL.Extensions.Tags.Validation(Client.id) },
                        Silent = false
                    });

                    try
                    {
                        response.EnsureSuccessStatusCode();
                    }
                    catch (Exception ex)
                    {
                        Utils.Diagnostic.Log(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.Diagnostic.Log(ex);
            }
        }

    }
}
