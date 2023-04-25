using InfraStarter.Configuration;
using InfraStarter.ViewModels;

namespace InfraStarter.Controls;

public partial class AzureKeyVaultFields : BaseResourceFields
{
	public AzureKeyVaultFields(AzureKeyVaultFieldsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		ViewModel = viewModel;
	}
}