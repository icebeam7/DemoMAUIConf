using DemoMAUIConf.ViewModels;

namespace DemoMAUIConf.Views;

public partial class ContactView : ContentPage
{
	public ContactView(ContactViewModel vm)
	{
		InitializeComponent();

        this.BindingContext = vm;
    }
}