using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfraStarter.Configuration;
using InfraStarter.Controls;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.ViewModels
{
	public partial class AzureKeyVaultFieldsViewModel : ResourceFieldsViewModel
	{
		private readonly AppConfig appConfig;
		private readonly IServiceProvider services;
		public AzureKeyVaultFieldsViewModel(AppConfig appConfig, IServiceProvider services)
		{
			this.appConfig = appConfig;
			this.services = services;
			AvailableIdentitySources = new ReadOnlyCollection<ResourceConfigSummaryViewModel>(
				appConfig.Resources.Values.Where(x => x.ResourceType == ResourceTypeName.AadApp).Select(x => new ResourceConfigSummaryViewModel(x)).ToList()
			);
		}

		public ReadOnlyCollection<ResourceConfigSummaryViewModel> AvailableIdentitySources { get; private set; }

		[ObservableProperty]
		public ResourceConfigSummaryViewModel selectedIdentitySource;
		/*
		[ObservableProperty]
		private string authenticationErrorMessage;

		[ObservableProperty]
		private IAccount authenticatedAccount;
		*/

		[ObservableProperty]
		private Guid pickerIdentitySourceId;

		[RelayCommand]
		public void EnablePicker()
		{
			PickerIdentitySourceId = SelectedIdentitySource.ID;
		}
		/*
		[RelayCommand]
		public async Task AuthenticateUser()
		{
			AuthenticationErrorMessage = null;
			if (SelectedIdentitySource == null)
			{
				return;
			}
			var config = appConfig.Resources[SelectedIdentitySource.ID] as IdentitySourceConfigAadApp;
			IPublicClientApplication app = PublicClientApplicationBuilder.Create(config.ApplicationID)
				.WithRedirectUri(config.RedirectURI)
				.WithTenantId(config.TenantID)
				.Build();
			try
			{
				var result = await app.AcquireTokenInteractive(new List<string> {
				@"https://graph.microsoft.com/User.Read" }).ExecuteAsync();
				AuthenticatedAccount = result.Account;
			}
			catch (Exception ex)
			{
				AuthenticationErrorMessage = ex.Message;
			}
		}
		*/
		[ObservableProperty]
		[Required(ErrorMessage = "Application ID is required")]
		private string appId;

		[ObservableProperty]
		[Required(ErrorMessage = "Redirect URI is required")]
		private string redirectUri;

		[ObservableProperty]
		private string tenantId;

		public override ResourceConfig OriginalData
		{
			set
			{
				if (value is IdentitySourceConfigAadApp)
				{
					var typed = value as IdentitySourceConfigAadApp;
					AppId = typed.ApplicationID;
					RedirectUri = typed.RedirectURI;
					TenantId = typed.TenantID;
				}
			}
		}

		public override IdentitySourceConfigAadApp CollectData(ResourceConfigCommon common)
		{
			ValidateAllProperties();
			if (HasErrors)
			{
				ValidationErrors = GetErrors();
				return null;
			}
			ValidationErrors = null;
			return new IdentitySourceConfigAadApp(common.ID, common.Name, ApplicationID: AppId, RedirectURI: RedirectUri)
			{
				TenantID = TenantId
			};
		}
	}
}
