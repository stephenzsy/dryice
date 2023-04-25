using CommunityToolkit.Mvvm.Input;
using InfraStarter.ViewModels;

namespace InfraStarter;

public partial class ResourcesPage : ContentPage
{
	public ResourcesPage(ResourcesPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}


}