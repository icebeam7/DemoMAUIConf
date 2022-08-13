using DemoMAUIConf.ViewModels;

namespace DemoMAUIConf.Views;

public partial class DeviceView : ContentPage
{
	public DeviceView(DeviceViewModel vm)
	{
		InitializeComponent();

        this.BindingContext = vm;
    }
}