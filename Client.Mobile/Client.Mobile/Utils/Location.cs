using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Mobile.Utils
{
    public static class Location
    {
        public static async Task<Xamarin.Essentials.Location> GetCurrentLocation(CancellationTokenSource cts)
        {

            Xamarin.Essentials.Location location = null;
            try
            {
                var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request, cts.Token);
            }
            catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (Xamarin.Essentials.FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (Xamarin.Essentials.PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            if (location == null)
                location = new Xamarin.Essentials.Location { Latitude = -1, Longitude = -1 };
            return location;
        }

    }
}
