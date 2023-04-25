using CommunityToolkit.Mvvm.ComponentModel;
using InfraStarter.Configuration;
using System.ComponentModel.DataAnnotations;

namespace InfraStarter.ViewModels.ResourceConfigForm
{
	internal partial class YubiKeyViewModel : ResourceConfigFormViewModelBase
	{
		public YubiKeyViewModel(AppConfig appConfig)
		{
			AppConfig = appConfig;
			AvailableIdentitySources = appConfig.Resources.Values.Where(x => x.ResourceType == ResourceTypeName.AadApp).Select(x => new ResourceConfigSummaryViewModel(x)).ToList().AsReadOnly();
		}

		public override ResourceConfig GetResourceConfig()
		{
			return new KeyProviderConfigAzureKeyVault(OriginalData == null || OriginalData is not KeyProviderConfigAzureKeyVault ? Guid.NewGuid() : OriginalData.ID, Name,
				KeyVaultURI: KeyVaultUri,
				IdentitySourceID: IdentitySource.ID);
		}

		[ObservableProperty]
		[Required]
		private string keyVaultUri;

		private AppConfig AppConfig { get; }

		[ObservableProperty]
		[Required]
		private ResourceConfigSummaryViewModel identitySource;

		[ObservableProperty]
		private IList<ResourceConfigSummaryViewModel> availableIdentitySources;

		protected override void ResetFromOriginalData(ResourceConfig value)
		{
			base.ResetFromOriginalData(value);
			if (value != null && value is KeyProviderConfigAzureKeyVault)
			{
				var v = value as KeyProviderConfigAzureKeyVault;
				KeyVaultUri = v.KeyVaultURI;
				IdentitySource = AvailableIdentitySources.Where(x => x.ID.Equals(v.IdentitySourceID)).FirstOrDefault();
			}
			else
			{
				KeyVaultUri = null;
				IdentitySource = null;
			}
		}
	}
}
