using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace Supplier.Mobile.Configration
{
    internal class MaterialConfigration
    {
        public static MaterialLoadingDialogConfiguration LoadingDialogConfiguration => new MaterialLoadingDialogConfiguration
        {
            BackgroundColor = Color.FromHex("#011A27"),
            MessageTextColor = Color.FromHex("#FFFFFF").MultiplyAlpha(0.8),
            TintColor = Color.FromHex("#FFFFFF"),
            CornerRadius = 8,
            ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32),
        };
        public static MaterialAlertDialogConfiguration AlertDialogConfiguration => new MaterialAlertDialogConfiguration
        {
            BackgroundColor = Color.FromHex("#011A27"),
            TitleTextColor = Color.FromHex("#FFFFFF"),
            MessageTextColor = Color.FromHex("#FFFFFF").MultiplyAlpha(0.8),
            TintColor = Color.FromHex("#FFFFFF"),
            CornerRadius = 8,
            ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32),
            ButtonAllCaps = false
        };

        public static MaterialSnackbarConfiguration SnackbarDialogConfiguration => new MaterialSnackbarConfiguration
        {
            BackgroundColor = Color.FromHex("#011A27"),
            MessageTextColor = Color.FromHex("#FFFFFF").MultiplyAlpha(0.8),
            TintColor = Color.FromHex("#FFFFFF"),
            CornerRadius = 8,
            ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32),
            ButtonAllCaps = false
        };
    }
}
