using InfraStarter.Pages;

namespace InfraStarter;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ResourcesPage), typeof(ResourcesPage));
		Routing.RegisterRoute(nameof(ResourceDetailsPage), typeof(ResourceDetailsPage));
		Routing.RegisterRoute(nameof(ProfileDetailsPage), typeof(ProfileDetailsPage));
	}
}
