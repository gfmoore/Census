namespace Census;

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
			});

		//dependency injection
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<DetailPage>();
		builder.Services.AddTransient<CensusViewModel>();
		builder.Services.AddTransient<CensusDetailViewModel>();

    //builder.Services.AddSingleton<IAlertService, AlertService>();

    return builder.Build();
	}
}
