using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Client.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupplierList : ContentPage
    {
        private IEnumerable<BLL.Models.Supplier> _Suppliers;
        public IEnumerable<BLL.Models.Supplier> Suppliers
        {
            get { return _Suppliers; }
            set { _Suppliers = value; OnPropertyChanged(); }
        }
        public System.Timers.Timer timer { get; set; }

        public SupplierList()
        {
            InitializeComponent();
            BindingContext = this;

            LoadConfigration().ConfigureAwait(false);
            timer = new System.Timers.Timer();
            timer.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
            timer.Elapsed += (s, e) => LoadConfigration().ConfigureAwait(false);
            timer.Start();
        }
        public async Task LoadConfigration()
        {
             var suppliers = await BLL.Services.FirebaseService.GetAllSuppliersAsync();
            var current_location = await Utils.Location.GetCurrentLocation(new CancellationTokenSource());
            foreach (var sup in suppliers)
            {
                var Origin = new BLL.Models.Location { latitude = sup.latitude, longitude = sup.longitude };
                var Distnation = new BLL.Models.Location { latitude = current_location.Latitude, longitude = current_location.Longitude };
                var TimeTreval = await BLL.Services.BingMaps.CalculateTimeTrevalAsync(Origin, Distnation);
                sup.estimated_time = TimeTreval.Add(TimeSpan.FromMinutes(30)).ToString(@"hh\:mm\:ss");
            }
            Suppliers = suppliers;
        }

        ~SupplierList()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}