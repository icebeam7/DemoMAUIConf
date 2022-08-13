using DemoMAUIConf.ViewModels;
using DemoMAUIConf.Views;

namespace DemoMAUIConf;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureEssentials(essentials =>
            {
                essentials.UseMapServiceToken("bingmaps-apikey");
            });

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IBattery>(Battery.Default);

		builder.Services.AddSingleton<IDeviceInfo>(DeviceInfo.Current);
        builder.Services.AddSingleton<IDeviceDisplay>(DeviceDisplay.Current);

        builder.Services.AddSingleton<IAppInfo>(AppInfo.Current);
        builder.Services.AddSingleton<IBrowser>(Browser.Default);

        builder.Services.AddSingleton<IContacts>(Contacts.Default);
        builder.Services.AddSingleton<IPhoneDialer>(PhoneDialer.Current);
        builder.Services.AddSingleton<ISms>(Sms.Default);
        builder.Services.AddSingleton<IEmail>(Email.Default);
        builder.Services.AddSingleton<IClipboard>(Clipboard.Default);
        builder.Services.AddSingleton<IFilePicker>(FilePicker.Default);
        builder.Services.AddSingleton<ITextToSpeech>(TextToSpeech.Default);
        builder.Services.AddSingleton<IShare>(Share.Default);
        builder.Services.AddSingleton<ILauncher>(Launcher.Default);
        builder.Services.AddSingleton<IScreenshot>(Screenshot.Default);
        builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
        builder.Services.AddSingleton<IFileSystem>(FileSystem.Current);
        
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);
        builder.Services.AddSingleton<IGeocoding>(Geocoding.Default);

        builder.Services.AddTransient<BatteryViewModel>();
        builder.Services.AddTransient<BatteryView>();

        builder.Services.AddTransient<DeviceViewModel>();
        builder.Services.AddTransient<DeviceView>();

        builder.Services.AddTransient<AppInfoViewModel>();
        builder.Services.AddTransient<AppInfoView>();

        builder.Services.AddTransient<ContactViewModel>();
        builder.Services.AddTransient<ContactView>();

        builder.Services.AddTransient<GPSViewModel>();
        builder.Services.AddTransient<GPSView>();

        return builder.Build();
	}
}
