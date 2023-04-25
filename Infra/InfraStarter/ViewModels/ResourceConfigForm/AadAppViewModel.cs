using CommunityToolkit.Mvvm.ComponentModel;
using InfraStarter.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.ViewModels.ResourceConfigForm
{
	internal partial class AadAppViewModel : ResourceConfigFormViewModelBase
	{

		[ObservableProperty]
		[Required]
		private string applicationId;

		[ObservableProperty]
		[Required]
		private string redirectUri;

		[ObservableProperty]
		private string tenantId;

		public override ResourceConfig GetResourceConfig()
		{
			return new IdentitySourceConfigAadApp(OriginalData == null || OriginalData is not IdentitySourceConfigAadApp ? Guid.NewGuid() : OriginalData.ID, Name, ApplicationId, RedirectUri)
			{
				TenantID = TenantId
			};
		}

		protected override void ResetFromOriginalData(ResourceConfig value)
		{
			base.ResetFromOriginalData(value);
			if (value != null && value is IdentitySourceConfigAadApp)
			{
				var v = value as IdentitySourceConfigAadApp;
				ApplicationId = v.ApplicationID;
				RedirectUri = v.RedirectURI;
				TenantId = v.TenantID;
			}
			else
			{
				ApplicationId = null;
				RedirectUri = null;
				TenantId = null;
			}
		}
	}
}
