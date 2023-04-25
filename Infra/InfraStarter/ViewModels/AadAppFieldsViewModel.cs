using CommunityToolkit.Mvvm.ComponentModel;
using InfraStarter.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.ViewModels
{
	public partial class AadAppFieldsViewModel : ResourceFieldsViewModel
	{
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
