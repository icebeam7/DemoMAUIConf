using DemoMAUIConf.ViewModels;

namespace DemoMAUIConf.Views;

public partial class GPSView : ContentPage
{
	public GPSView(GPSViewModel vm)
	{
		InitializeComponent();

		this.BindingContext = vm;
	}
}