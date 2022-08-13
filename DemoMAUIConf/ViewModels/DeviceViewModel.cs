using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoMAUIConf.ViewModels
{
    public partial class DeviceViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IDeviceInfo deviceInfo;

        [ObservableProperty]
        private IDeviceDisplay deviceDisplay;

        public DeviceViewModel(IDeviceInfo deviceInfo, IDeviceDisplay deviceDisplay)
        {
            this.deviceInfo = deviceInfo;
            this.deviceDisplay = deviceDisplay;
            this.deviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
        }

        private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DeviceDisplay));
        }
    }
}
