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
    public partial class SignInPage : ContentPage
    {
        private readonly Lazy<IFirebaseAuth> _firebaseAuth;
        private IFirebaseAuth _fireAuth;
        public SignInPage()
        {
            InitializeComponent();
            _firebaseAuth = new Lazy<IFirebaseAuth>(CreateFirebaseAuth);
        }
        private static IFirebaseAuth CreateFirebaseAuth() => CrossFirebaseAuth.Current;

        private async void SendOTPCode(object sender, EventArgs e)
        {
            try
            {
                _fireAuth = _firebaseAuth.Value;
                await _fireAuth.VerifyPhoneNumberAsync(TextPhoneNumber.Text);
                await Navigation.PushAsync(new OTPVerifyPage(_fireAuth, TextPhoneNumber.Text));
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
    }
}