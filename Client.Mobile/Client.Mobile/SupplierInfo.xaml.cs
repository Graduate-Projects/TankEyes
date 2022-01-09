using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace Client.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupplierInfo : ContentPage
    {
        public Order OrderRequest { get; }
        public Supplier Supplier_Profile { get; }
        public ICommand DialUpCommand { get; set; }

        private string _estimated_time = "--:--:--";
        public string estimated_time
        {
            get { return _estimated_time; }
            set { _estimated_time = value; OnPropertyChanged(); }
        }
        private int _number_order_finished;
        public int number_order_finished
        {
            get { return _number_order_finished; }
            set { _number_order_finished = value; OnPropertyChanged(); }
        }

        public System.Timers.Timer TimerCalculateRemainTime { get; private set; }

        public SupplierInfo(BLL.Models.Order order_watting, BLL.Models.Supplier supplier_profile)
        {
            InitializeComponent();
            OrderRequest = order_watting;
            Supplier_Profile = supplier_profile;
            DialUpCommand = new Command(() => DialUp().ConfigureAwait(false));
            BindingContext = this;

            TimerCalculateRemainTime = new System.Timers.Timer();
            TimerCalculateRemainTime.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
            TimerCalculateRemainTime.Elapsed += (s, e) => LoadNewLocation().ConfigureAwait(false);
            TimerCalculateRemainTime.Start();

            LoadOrderSupplier(supplier_profile.id).ConfigureAwait(false);

            MessagingCenter.Subscribe<App>(this,"OrderHasCancelled", (sender) => {
                App.Current.MainPage.Navigation.PopAsync();
            });
        }
        private async Task DialUp()
        {
            try
            {
                Xamarin.Essentials.PhoneDialer.Open(Supplier_Profile.phone_number);
            }
            catch (ArgumentNullException anEx)
            {

            }
            catch (Xamarin.Essentials.FeatureNotSupportedException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }
        private async Task LoadOrderSupplier(string supplier_id)
        {
            var orders = await BLL.Services.FirebaseService.GetOrdersSupplierAsync(Supplier_Profile.id);
            number_order_finished = orders.Count(item => item.status == BLL.Enums.OrderStatus.Done);
        }
        private async Task LoadNewLocation()
        {
            var current_location = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());

            var Origin = new BLL.Models.Location { latitude = Supplier_Profile.latitude, longitude = Supplier_Profile.longitude };
            var Distnation = new BLL.Models.Location { latitude = current_location.Latitude, longitude = current_location.Longitude };
            var TimeTreval = await BLL.Services.BingMaps.CalculateTimeTrevalAsync(Origin, Distnation);
            estimated_time = TimeTreval.Add(TimeSpan.FromMinutes(30)).ToString(@"hh\:mm\:ss");
        }

        private async void CancelledOrder(object sender, EventArgs e)
        {
            var result = await MaterialDialog.Instance.ConfirmAsync(message: "Are you sure you want to cancel this order?", confirmingText: "Yes", dismissiveText: "No");
            if (result != null)
            {
                if (result.Value)
                {
                    OrderRequest.status = BLL.Enums.OrderStatus.Cancelled;
                    BLL.Services.FirebaseService.UpdateOrder(OrderRequest.id, OrderRequest);
                    SendNotification("Order has been cancelled", $"It looks like customer canceled the order for some reason, we apologize and appreciate your effort");
                    await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => App.Current.MainPage.Navigation.PopAsync());
                }
            }
        }
        private async void FinishOrder(object sender, EventArgs e)
        {
            var result = await MaterialDialog.Instance.ConfirmAsync(message: "Are you sure you want to finish this order?", confirmingText: "Yes", dismissiveText: "No");
            if (result != null)
            {
                if (result.Value)
                {
                    OrderRequest.status = BLL.Enums.OrderStatus.Done;
                    BLL.Services.FirebaseService.UpdateOrder(OrderRequest.id, OrderRequest);
                    SendNotification("Order has been Finished", $"Thank you for fulfilling this request, we are glad to have you in our family");
                    await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => App.Current.MainPage.Navigation.PopAsync());
                }
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
                        Tags = new string[] { BLL.Extensions.Tags.Validation(Supplier_Profile.id) },
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