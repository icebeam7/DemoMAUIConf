using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DemoMAUIConf.ViewModels
{
    public partial class GPSViewModel : BaseViewModel
    {
        [ObservableProperty]
        private double latitude;

        [ObservableProperty]
        private double longitude;

        [ObservableProperty]
        private string address;

        private CancellationTokenSource cts;
        private IGeolocation geolocation;
        private IGeocoding geocoding;
        private IMap map;

        public GPSViewModel(IGeolocation geolocation, IGeocoding geocoding, IMap map)
        {
            this.geolocation = geolocation;
            this.geocoding = geocoding;
            this.map = map;
        }

        [RelayCommand]
        private async Task GetCurrentLocation()
        {
            try
            {
                cts = new CancellationTokenSource();

                var request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10));

                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                Latitude = location.Latitude;
                Longitude = location.Longitude;
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        [RelayCommand]
        private async Task ShowExternalMap()
        {
            var options = new MapLaunchOptions()
            {
                Name = "Current location",
                NavigationMode = NavigationMode.None
            };

            await map.OpenAsync(latitude, longitude, options);
        }

        [RelayCommand]
        private async Task GetLocationFromAddress()
        {
            try
            {
                var locations = await Geocoding.GetLocationsAsync(address);
                var location = locations.FirstOrDefault();

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                }
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }
        }

        [RelayCommand]
        private async Task GetAddressFromLocation()
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {
                    Address =
                        $"AdminArea:       {placemark.AdminArea}\n" +
                        $"CountryCode:     {placemark.CountryCode}\n" +
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"FeatureName:     {placemark.FeatureName}\n" +
                        $"Locality:        {placemark.Locality}\n" +
                        $"PostalCode:      {placemark.PostalCode}\n" +
                        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                        $"SubLocality:     {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare:    {placemark.Thoroughfare}\n";
                }
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }

        }

        [RelayCommand]
        private void DisposeCancellationToken()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
        }
    }
}
