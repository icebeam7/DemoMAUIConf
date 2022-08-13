using DemoMAUIConf.ViewModels;

namespace DemoMAUIConf.Views;

public partial class BatteryView : ContentPage
{
	public BatteryView(BatteryViewModel vm)
	{
		InitializeComponent();

        this.BindingContext = vm;
    }
}