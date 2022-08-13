using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DemoMAUIConf.ViewModels
{
    public partial class BatteryViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IBattery battery;

        private IConnectivity connectivity;

        public BatteryViewModel(IBattery battery, IConnectivity connectivity)
        {
            this.battery = battery;
            this.connectivity = connectivity;

            this.battery.BatteryInfoChanged += Battery_BatteryInfoChanged;
            this.battery.EnergySaverStatusChanged += Battery_EnergySaverStatusChanged;
        }

        private void Battery_EnergySaverStatusChanged(object sender, EnergySaverStatusChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Battery));
        }

        private void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Battery));
        }

        [RelayCommand]
        private async Task DownloadDatabaseAsync()
        {
            if (connectivity.NetworkAccess == NetworkAccess.Internet)
            {

                bool canUpdate;

                switch (battery.PowerSource)
                {
                    case BatteryPowerSource.AC:
                    case BatteryPowerSource.Usb:
                    case BatteryPowerSource.Wireless:
                        canUpdate = true;
                        break;
                    case BatteryPowerSource.Battery:
                        canUpdate = battery.ChargeLevel > 0.80;
                        break;
                    default:
                        canUpdate = false;
                        break;
                }

                await App.Current.MainPage.DisplayAlert(
                    "Updating local database...",
                    canUpdate
                        ? "The database is being downloaded"
                        : "Please connect your device to the power source. The database can't be updated right now",
                    "OK");

                if (canUpdate)
                {
                    await Task.Delay(4000);

                    try
                    {
                        var duration = TimeSpan.FromMilliseconds(2500);
                        Vibration.Vibrate(duration);
                        await App.Current.MainPage.DisplayAlert("Info", "Database updated", "OK");
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Info", "Database updated (no vibration)", "OK");
                    }
                }
            }
            else
                await App.Current.MainPage.DisplayAlert("Error", "You need an Internet connection", "OK");
        }
    }
}
