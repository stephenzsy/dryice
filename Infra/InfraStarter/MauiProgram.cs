using CommunityToolkit.Maui;
using InfraStarter.Configuration;
using InfraStarter.Controls;
using InfraStarter.Pages;
using InfraStarter.Services;
using InfraStarter.Services.Profile;
using InfraStarter.ViewModels;
using InfraStarter.ViewModels.ResourceConfigForm;
using Microsoft.Extensions.Logging;

namespace InfraStarter;

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
				fonts.AddFont("materialdesignicons-webfont.ttf", "Mdi");
			});

		builder.Services
			.AddSingleton<IProfileService, ProfileService>()
			.AddSingleton<SettingsPageViewModel>()
			.AddSingleton<SettingsPage>()
			.AddTransient<ProfileDetailsPageViewModel>()
			.AddSingleton<ProfileDetailsPage>()
			.AddSingleton<AppConfig>(AppConfig.LoadFromAppData())
			.AddSingleton<ResourcesPageViewModel>()
			.AddSingleton<ResourcesPage>()
			.AddSingleton<ResourceConfigFormTemplates>()
			.AddSingleton<ResourceDetailsPageViewModel>()
			.AddSingleton<ResourceDetailsPage>()
			.AddSingleton<AzureKeyVaultFieldsViewModel>()
			.AddSingleton<AzureKeyVaultFields>()
			.AddTransient<KeyVaultPickerViewModel>()
			.AddTransient<AadAppViewModel>()
			.AddTransient<AzureKeyVaultViewModel>()
			.AddTransient<YubiKeyViewModel>();
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
