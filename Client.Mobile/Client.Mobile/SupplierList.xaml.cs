using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Client.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupplierList : ContentPage
    {
        private BLL.Models.Client _client { get; set; }
        public ICommand SendOrderRequestCommand { get; set; }

        private List<BLL.Models.Supplier> _Suppliers;
        public List<BLL.Models.Supplier> Suppliers
        {
            get { return _Suppliers; }
            set { _Suppliers = value; OnPropertyChanged(); }
        }
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public System.Timers.Timer timer { get; set; }

        public SupplierList(BLL.Models.Client client)
        {
            InitializeComponent();
            BindingContext = this;
            _client = client;
            SendOrderRequestCommand = new Command<BLL.Models.Supplier>((supplier) => SendOrderRequest(supplier));
            LoadOrderWatting().ConfigureAwait(false);
            LoadConfigration().ConfigureAwait(false);
            
            timer = new System.Timers.Timer();
            timer.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
            timer.Elapsed += (s, e) => LoadConfigration().ConfigureAwait(false);
            timer.Start();
        }

        private async Task LoadOrderWatting(bool can_show_message = true)
        {
            var order_client = await BLL.Services.FirebaseService.GetOrdersClientsAsync(_client.id);
            var order_watting = order_client.FirstOrDefault(item => item.status == BLL.Enums.OrderStatus.Watting);
            if (order_watting != null)
            {
                if (order_watting.is_supplier_accepted)
                {
                    var supplier_profile = await BLL.Services.FirebaseService.GetSupplier(order_watting.supplier_id);
                    await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(async() =>
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new SupplierInfo(order_watting, supplier_profile));
                        App.Current.MainPage.Navigation.RemovePage(this);
                    });
                }
                else
                {
                   if(can_show_message) UI_ORDER_DESIGN(order_watting.id, order_watting);
                }
            }
        }

        public async Task LoadConfigration()
        {
            IsLoading = true;
            var suppliers = await BLL.Services.FirebaseService.GetAllSuppliersAsync();
            var suppliers_after_filter = new List<BLL.Models.Supplier>();
            var current_location = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
            foreach (var sup in suppliers)
            {

                var orders_suppliers = await BLL.Services.FirebaseService.GetOrdersSupplierAsync(sup.id);
                if (sup.work_location == _client.home_location && orders_suppliers.All(item => item.status != BLL.Enums.OrderStatus.Watting))
                {
                    var Origin = new BLL.Models.Location { latitude = sup.latitude, longitude = sup.longitude };
                    var Distnation = new BLL.Models.Location { latitude = current_location.Latitude, longitude = current_location.Longitude };
                    var TimeTreval = await BLL.Services.BingMaps.CalculateTimeTrevalAsync(Origin, Distnation);
                    sup.estimated_time = TimeTreval.Add(TimeSpan.FromMinutes(30)).ToString(@"hh\:mm\:ss");
                    suppliers_after_filter.Add(sup);
                }
            }

            Suppliers = suppliers_after_filter.OrderByDescending(item=>item.available).ToList();
            RefreshViewSupplier.IsRefreshing = false;
            IsLoading = false;
        }

        ~SupplierList()
        {
            timer.Stop();
            timer.Dispose();
        }

        private async void SendOrderRequest(BLL.Models.Supplier supplier)
        {
            var order_id = $"{Guid.NewGuid()}";
            var order = new BLL.Models.Order
            {
                id = order_id,
                creation_date = DateTime.Now.ToString("G"),
                client_id = _client.id,
                supplier_id = supplier.id,
                is_supplier_accepted = false,
                status = BLL.Enums.OrderStatus.Watting
            };
            await BLL.Services.FirebaseService.AddNewOrder(order_id, order).ConfigureAwait(false);
            SendNotification("New Order", "You have new order, please check it..", supplier.id);
            UI_ORDER_DESIGN(order_id, order);

        }
        private void UI_ORDER_DESIGN(string order_id, BLL.Models.Order order)
        {
            var loading_timer = new System.Timers.Timer();
            var start_time = DateTime.Now;
            loading_timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            loading_timer.Elapsed += async (s, e) =>
            {
                if (!(App.Current.MainPage.Navigation.NavigationStack.Last() is SupplierList))
                {
                    loading_timer.Stop();
                    loading_timer.Dispose();
                }
                else
                {
                    var period_time = 3 * 60 - (int)DateTime.Now.Subtract(start_time).TotalSeconds;
                    if (period_time % 15 == 0) await LoadOrderWatting(false).ConfigureAwait(false); //CHECK UPDATE ORDERING EVERY 15 SECONDS
                    if (period_time >= 60)
                    {
                        await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() =>
                        MessageText.Text = String.Format("remain {0:c} before cancelled your request", TimeSpan.FromSeconds(period_time)));
                    }
                    else
                    {
                        loading_timer.Stop();
                        order.status = BLL.Enums.OrderStatus.Cancelled;
                        await BLL.Services.FirebaseService.UpdateOrder(order_id, order).ConfigureAwait(false);
                        await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => MessageFrame.IsVisible = false);
                        loading_timer.Dispose();
                    }
                }
            };
            loading_timer.Start();
            Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => MessageFrame.IsVisible = true);
        }
        private void RefreshListSupplier(object sender, EventArgs e)
        {
            LoadConfigration().ConfigureAwait(false);
        }
        private async void SendNotification(string title, string message,string tag)
        {
            try
            {
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://tank-eyes.azurewebsites.net/api/Notifications/requests", new BLL.Models.Notification.NotificationRequest
                    {
                        Title = title,
                        Text = message,
                        Tags = new string[] { BLL.Extensions.Tags.Validation(tag) },
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