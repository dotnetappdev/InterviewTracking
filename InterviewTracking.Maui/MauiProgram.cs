using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Maui;
using InterviewTracking.Maui.Data;
using InterviewTracking.Maui.Services;
using InterviewTracking.Maui.ViewModels;
using InterviewTracking.Maui.Views;

namespace InterviewTracking.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Configure SQLite database
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "interviews.db");
		builder.Services.AddDbContext<LocalDbContext>(options =>
			options.UseSqlite($"Data Source={dbPath}"));

		// Register Services
		builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
		builder.Services.AddSingleton<IPreferences>(Preferences.Default);
		builder.Services.AddHttpClient<IApiService, ApiService>(client =>
		{
			client.BaseAddress = new Uri("https://localhost:7000/api/"); // Update with your API URL
		});
		builder.Services.AddScoped<IInterviewLocalService, InterviewLocalService>();
		builder.Services.AddScoped<IAuthLocalService, AuthLocalService>();
		builder.Services.AddScoped<ISyncService, SyncService>();
		builder.Services.AddScoped<INotificationService, NotificationService>();

		// Register ViewModels
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<RegisterViewModel>();
		builder.Services.AddTransient<InterviewListViewModel>();
		builder.Services.AddTransient<InterviewDetailViewModel>();
		builder.Services.AddTransient<AddEditInterviewViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();

		// Register Views
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<InterviewListPage>();
		builder.Services.AddTransient<InterviewDetailPage>();
		builder.Services.AddTransient<AddEditInterviewPage>();
		builder.Services.AddTransient<SettingsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
