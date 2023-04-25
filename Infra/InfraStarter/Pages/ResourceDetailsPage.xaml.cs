using InfraStarter.Configuration;
using InfraStarter.ViewModels;
using Microsoft.Maui.Controls;

namespace InfraStarter;

[QueryProperty(nameof(ID), ParamID)]
public partial class ResourceDetailsPage : ContentPage
{
	private readonly ResourceDetailsPageViewModel vm;

	public ResourceDetailsPage(ResourceDetailsPageViewModel vm)
	{
		this.vm = vm;
		InitializeComponent();
		BindingContext = vm;
	}

	public Guid ID
	{
		set
		{
			vm.ID = value;
		}
	}

	public const string ParamID = "id";
}