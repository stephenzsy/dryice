namespace InfraStarter;

public partial class App : Application
{
	public App(IServiceProvider services)
	{
		Services = services;
		InitializeComponent();
		MainPage = new AppShell();
	}

	public IServiceProvider Services { get; private set; }
}
