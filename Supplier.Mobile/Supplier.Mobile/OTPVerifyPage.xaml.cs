using Firebase.Database;
using Plugin.Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Supplier.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OTPVerifyPage : ContentPage
    {
        private IFirebaseAuth _firebaseAuth;
        public string PIN { get; set; }
        public string PhoneNumber { get; set; }
        public OTPVerifyPage(IFirebaseAuth _fireAuth,string PhoneNumber)
        {
            InitializeComponent();
            this.BindingContext = this;
            this._firebaseAuth = _fireAuth;
            this.PhoneNumber = PhoneNumber;
        }
        private async void Verify_OTP(string OTPCode)
        {
            try
            {
                var result = await _firebaseAuth.SignInWithPhoneNumberVerificationCodeAsync(OTPCode);
                if (result != null && result.Uid != null)
                {
                    var suppliers = await BLL.Services.FirebaseService.GetAllSuppliersAsync();
                    var supplierProfile = suppliers.FirstOrDefault(spp => spp.phone_number == PhoneNumber);
                    if(supplierProfile == null)
                        App.Current.MainPage = new SingUpPage(PhoneNumber);
                    else
                        App.Current.MainPage = new NavigationPage(new MainPage(supplierProfile));
                }
            }
            catch (FirebaseException ex)
            {
                await DisplayAlert("Error", ex.Message, "Okay");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        private void PINEntryCompleted(object sender, XFPINView.Helpers.PINCompletedEventArgs e)
        {
            Verify_OTP(e.PIN);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}