using DemoMAUIConf.ViewModels;

namespace DemoMAUIConf.Views;

public partial class AppInfoView : ContentPage
{
	public AppInfoView(AppInfoViewModel vm)
	{
		InitializeComponent();

        this.BindingContext = vm;
    }
}