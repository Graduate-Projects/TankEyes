using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Supplier.Mobile
{
    public partial class MainPage : ContentPage
    {
        public BLL.Models.Supplier Supplier { get; set; }
        public MainPage(BLL.Models.Supplier Profile)
        {
            Supplier = Profile;
            InitializeComponent();
            this.BindingContext = this;
        }
    }
}
