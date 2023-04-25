using InfraStarter.Configuration;
using InfraStarter.ViewModels;

namespace InfraStarter.Controls;

public partial class AadAppFields : BaseResourceFields
{
	public AadAppFields()
	{
		InitializeComponent();
		var viewModel = new AadAppFieldsViewModel();
		BindingContext = viewModel;
		ViewModel = viewModel;
	}
}