using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Supplier.Mobile
{
    public partial class MainPage : ContentPage
    {
        public BLL.Models.Supplier Supplier { get; set; }
        public Timer Timer { get; private set; }

        public MainPage(BLL.Models.Supplier Profile)
        {
            Supplier = Profile;
            InitializeComponent();
            this.BindingContext = this;

            Timer = new System.Timers.Timer();
            Timer.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
            Timer.Elapsed += (s, e) => UpdateCurrentLocation().ConfigureAwait(false);
            Timer.Start();
        }
        public async Task UpdateCurrentLocation()
        {
            var CurentLocation = await Utils.Location.GetCurrentLocation(new System.Threading.CancellationTokenSource ());
            Supplier.latitude = CurentLocation.Latitude;
            Supplier.longitude = CurentLocation.Longitude;
            await BLL.Services.FirebaseService.UpdateSupplier(Supplier.id, Supplier).ConfigureAwait(false);
        }

        private async void SendNotification(object sender, EventArgs e)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://tank-eyes.azurewebsites.net/api/Notifications/requests", new BLL.Models.Notification.NotificationRequest
                    {
                        Title = "Notification",
                        Text = "Welcome On Tank Eyes",
                        Tags = new string[] { "dc24bf224ba74be69b7e261d12dc69a7" },
                        Silent = false
                    });

                    try
                    {
                        response.EnsureSuccessStatusCode();
                        await MaterialDialog.Instance.SnackbarAsync(message: "Notification has been sent", msDuration: MaterialSnackbar.DurationLong);
                    }
                    catch (Exception ex)
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: $"Notification Failed: {response.StatusCode} Error", msDuration: MaterialSnackbar.DurationLong);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("",ex.ToString(),"Ok");
            }
        }

        private void ToggleAvailability(object sender, EventArgs e)
        {
            Supplier.available = !Supplier.available;
            BLL.Services.FirebaseService.UpdateSupplier(Supplier.id, Supplier).ConfigureAwait(false);
            MaterialDialog.Instance.SnackbarAsync(message: "Updated Availability", msDuration: MaterialSnackbar.DurationLong);
        }
    }
}
