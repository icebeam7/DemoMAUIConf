using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoMAUIConf.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string title;
    }
}
