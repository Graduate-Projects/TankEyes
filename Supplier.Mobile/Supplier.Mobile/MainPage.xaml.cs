using MediaManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Supplier.Mobile
{
    public partial class MainPage : ContentPage
    {
        public BLL.Models.Supplier Supplier { get; set; }

        private BLL.Models.Client _ClientRequest;
        public BLL.Models.Client ClientRequest
        {
            get { return _ClientRequest; }
            set { _ClientRequest = value; OnPropertyChanged(); }
        }
        private BLL.Models.Order _OrderRequest;
        public BLL.Models.Order OrderRequest
        {
            get { return _OrderRequest; }
            set { _OrderRequest = value; OnPropertyChanged(); }
        }
        public ICommand AcceptRequestCommand { get; set; }
        public ICommand RejectRequestCommand { get; set; }
        public ICommand CancelledRequestCommand { get; set; }
        public ICommand OpenDirectionCommand { get; set; }
        public ICommand DialUpCommand { get; set; }

        public Timer TimerUpdateLocation { get; private set; }
        public Timer TimerRefreshOrder { get; private set; }

        public MainPage(BLL.Models.Supplier Profile)
        {
            Supplier = Profile;
            InitializeComponent();
            AcceptRequestCommand = new Command(() => AcceptRequest());
            RejectRequestCommand = new Command(() => RejectRequest());
            CancelledRequestCommand = new Command(() => CancelledRequest());
            OpenDirectionCommand = new Command(() => OpenDirection().ConfigureAwait(false));
            DialUpCommand = new Command(() => DialUp().ConfigureAwait(false));;
            this.BindingContext = this;

            TimerUpdateLocation = new System.Timers.Timer();
            TimerUpdateLocation.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
            TimerUpdateLocation.Elapsed += (s, e) => UpdateCurrentLocation().ConfigureAwait(false);
            TimerUpdateLocation.Start();

            TimerRefreshOrder = new System.Timers.Timer();
            TimerRefreshOrder.Interval = TimeSpan.FromSeconds(15).TotalMilliseconds;
            TimerRefreshOrder.Elapsed += (s, e) => LoadOrderWatting().ConfigureAwait(false);
            TimerRefreshOrder.Start();
        }
        private async Task DialUp()
        {
            try
            {
                PhoneDialer.Open(ClientRequest.phone_number);
            }
            catch (ArgumentNullException anEx)
            {

            }
            catch (FeatureNotSupportedException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }
        private async Task OpenDirection()
        {
            var location = new Location(ClientRequest.latitude , ClientRequest.longitude);
            var options = new MapLaunchOptions { Name = $"Home Order: {ClientRequest.full_name}" };

            try
            {
                await Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {

            }
        }

        private async void AcceptRequest()
        {
            RequestOrder.IsVisible = false;
            OrderRequest.is_supplier_accepted = true;
            OrderRequest.status = BLL.Enums.OrderStatus.Watting;
            BLL.Services.FirebaseService.UpdateOrder(OrderRequest.id, OrderRequest);
            ClientRequest = await BLL.Services.FirebaseService.GetClient(OrderRequest.client_id);
            MessageSearch.IsVisible = false;
            InfoOrder.IsVisible = true;

            SendNotification("Order has been accepted", $"The service provider ({Supplier.full_name}) is going to your house right now, thank you");
        }

        private void RejectRequest()
        {
            RequestOrder.IsVisible = false;
            OrderRequest.is_supplier_accepted = false;
            OrderRequest.status = BLL.Enums.OrderStatus.Rejected;
            BLL.Services.FirebaseService.UpdateOrder(OrderRequest.id, OrderRequest);
            MessageSearch.IsVisible = true;

            SendNotification("Order has been rejected", $"The service provider ({Supplier.full_name}) reject your order due to some circumstance, we apologize");
        }
        private async void CancelledRequest()
        {
            var result = await MaterialDialog.Instance.ConfirmAsync(message: "Are you sure you want to cancel this order?", confirmingText: "Yes, Sure", dismissiveText: "No");
            if (result != null)
            {
                if (result.Value)
                {
                    MessageSearch.IsVisible = true;
                    OrderRequest.status = BLL.Enums.OrderStatus.Cancelled;
                    BLL.Services.FirebaseService.UpdateOrder(OrderRequest.id, OrderRequest);
                    InfoOrder.IsVisible = false;
                    SendNotification("Order has been cancelled", $"The service provider ({Supplier.full_name}) canceled your order due to some circumstance, we apologize");
                }
            }
        }

        private async Task LoadOrderWatting()
        {
            var order_supplier = await BLL.Services.FirebaseService.GetOrdersSupplierAsync(Supplier.id);
            var is_reserved = order_supplier.Any(item => item.status == BLL.Enums.OrderStatus.Watting && item.is_supplier_accepted);
            if (!is_reserved)
            {
                var order_watting = order_supplier.FirstOrDefault(item => item.status == BLL.Enums.OrderStatus.Watting);
                if (order_watting != null)
                {
                    await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        CrossMediaManager.Current.Play("notification.mp3");
                        MessageSearch.IsVisible = false;
                        RequestOrder.IsVisible = true;
                        OrderRequest = order_watting;
                    });

                }
            }
            else
            {
                var order_reserved = order_supplier.First(item => item.status == BLL.Enums.OrderStatus.Watting && item.is_supplier_accepted);
                var client_info = await BLL.Services.FirebaseService.GetClient(order_reserved.client_id);
                await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() =>
                {
                    MessageSearch.IsVisible = false;
                    OrderRequest = order_reserved;
                    ClientRequest = client_info;
                    InfoOrder.IsVisible = true;
                    //add this client to list clients with cancelled button
                });
            }
        }

        public async Task UpdateCurrentLocation()
        {
            var CurentLocation = await Utils.Location.GetCurrentLocation(new System.Threading.CancellationTokenSource ());
            Supplier.latitude = CurentLocation.Latitude;
            Supplier.longitude = CurentLocation.Longitude;
            await BLL.Services.FirebaseService.UpdateSupplier(Supplier.id, Supplier).ConfigureAwait(false);
        }

        private async void ToggleAvailability(object sender, ToggledEventArgs e)
        {
            await BLL.Services.FirebaseService.UpdateSupplier(Supplier.id, Supplier);
        }

        private async void SendNotification(string title, string message)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://tank-eyes.azurewebsites.net/api/Notifications/requests", new BLL.Models.Notification.NotificationRequest
                    {
                        Title = title,
                        Text = message,
                        Tags = new string[] { ClientRequest.id.Replace("-","") },
                        Silent = false
                    });

                    try
                    {
                        response.EnsureSuccessStatusCode();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
