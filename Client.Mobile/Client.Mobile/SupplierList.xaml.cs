using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
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
        public Timer timer { get; set; }

        public SupplierList()
        {
            InitializeComponent();
            BindingContext = this;

            LoadConfigration().ConfigureAwait(false);
            timer = new Timer();
            timer.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
            timer.Elapsed += (s, e) => LoadConfigration().ConfigureAwait(false);
            timer.Start();
        }
        public async Task LoadConfigration()
        {
            Suppliers = await BLL.Services.FirebaseService.GetAllSuppliersAsync();
        }

        ~SupplierList()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}