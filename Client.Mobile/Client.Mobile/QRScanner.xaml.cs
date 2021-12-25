using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace Client.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRScanner : ContentPage
    {
        public QRScanner()
        {
            InitializeComponent();

        }

        private void OnScanResult(Result result)
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Stop analysis until we navigate away so we don't keep reading barcodes
                    ZXingScanner.IsAnalyzing = false;
                    if (Guid.TryParse(result.Text, out Guid output))
                    {
                        await Xamarin.Essentials.SecureStorage.SetAsync("ClientID", $"{output}");

                        AppStatic.ClientID = $"{output}";

                        var client_info = await FirebaseService.GetClient(AppStatic.ClientID);
                        if (client_info == null)
                            await Navigation.PushAsync(new RegisterInfo());
                        else
                            Application.Current.MainPage = new NavigationPage(new MainPage());
                    }
                    else
                    {
                        ZXingScanner.IsAnalyzing = true;
                    }
                });
            }
            catch (Exception)
            {
                ZXingScanner.IsAnalyzing = true;
            }
        }

        private void FlashToggle(Button sender, EventArgs e)
        {
            ZXingScanner.IsTorchOn = !ZXingScanner.IsTorchOn;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ZXingScanner.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            ZXingScanner.IsScanning = false;

            base.OnDisappearing();
        }
    }
}