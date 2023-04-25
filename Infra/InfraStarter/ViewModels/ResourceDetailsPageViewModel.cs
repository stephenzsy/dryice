using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfraStarter.Configuration;
using InfraStarter.Controls;
using InfraStarter.ViewModels.ResourceConfigForm;
using System.ComponentModel.DataAnnotations;

namespace InfraStarter.ViewModels
{
	public partial class ResourceDetailsPageViewModel : ObservableObject
	{
		[ObservableProperty]
		private string resourceType;
	}

	public partial class ResourceDetailsPageViewModel : ObservableObject
	{
		private readonly AppConfig appConfig;
		private IServiceProvider Services { get; }
		public ResourceConfigFormTemplates ConfigFormResources { get; }

		private Guid resourceID;

		public ResourceDetailsPageViewModel(AppConfig config, IServiceProvider services, ResourceConfigFormTemplates configFormResources)
		{
			appConfig = config;
			Services = services;
			ConfigFormResources = configFormResources;
		}

		public Guid ID
		{
			get => resourceID;
			set
			{
				resourceID = value;
				if (Guid.Empty.Equals(value) || !appConfig.Resources.ContainsKey(value))
				{
					PageTitle = "Create resource";
					IsNew = true;
					ResourceType = string.Empty;
				}
				else
				{
					var resourceConfig = appConfig.Resources[value];
					PageTitle = "Edit resource";
					IsNew = false;
					ResourceType = resourceConfig.ResourceType;
					FormViewModel.OriginalData = resourceConfig;
				}
			}
		}


		[ObservableProperty]
		private string pageTitle;

		[ObservableProperty]
		private bool isNew;

		[ObservableProperty]
		private object formControlTemplate;

		[ObservableProperty]
		private ResourceConfigFormViewModelBase formViewModel;

		partial void OnResourceTypeChanged(string value)
		{
			switch (value)
			{
				case ResourceTypeName.AadApp:
					FormControlTemplate = ConfigFormResources["ResourceConfigFormAadApp"];
					FormViewModel = Services.GetService<AadAppViewModel>();
					break;
				case ResourceTypeName.AzureKeyVault:
					FormControlTemplate = ConfigFormResources["ResourceConfigFormAzureKeyVault"];
					FormViewModel = Services.GetService<AzureKeyVaultViewModel>();
					break;
				case ResourceTypeName.YubiKey:
					FormControlTemplate = ConfigFormResources["ResourceConfigFormYubiKey"];
					FormViewModel = Services.GetService<YubiKeyViewModel>();
					break;
				default:
					FormControlTemplate = null;
					FormViewModel = null;
					break;
			}
		}

		[ObservableProperty]
		public IList<ValidationResult> validationErrors;

		[RelayCommand]
		public void Save()
		{
			ValidationErrors = FormViewModel?.ValidateForm();
			if (ValidationErrors == null || !ValidationErrors.Any())
			{
				var collected = FormViewModel?.GetResourceConfig();
				if (collected != null)
				{
					appConfig.SaveResource(collected);
				}
			}
		}

		[RelayCommand]
		public void Cancel()
		{
			Shell.Current.SendBackButtonPressed();
		}

	}

}
