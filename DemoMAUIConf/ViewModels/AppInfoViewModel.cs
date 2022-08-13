using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DemoMAUIConf.ViewModels
{
    public partial class AppInfoViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IAppInfo appInfo;

        private IBrowser browser;

        public AppInfoViewModel(IAppInfo appInfo, IBrowser browser)
        {
            this.appInfo = appInfo;
            this.browser = browser;            
        }

        [RelayCommand]
        private void DisplaySettings()
        {
            appInfo.ShowSettingsUI();
        }

        [RelayCommand]
        private async Task VisitWebsiteAsync()
        {
            try
            {
                var url = "https://luisbeltran.mx";
                var blueColor = Colors.Blue;
                var blueColorWithAlpha = blueColor.MultiplyAlpha(0.5f);

                var options = new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredControlColor = Colors.Green,
                    PreferredToolbarColor = blueColorWithAlpha
                };

                await browser.OpenAsync(url, options);
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Info", "No browser installed", "OK");
            }
        }
    }
}
