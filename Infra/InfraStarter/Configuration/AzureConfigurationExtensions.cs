using Azure.Core;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Configuration
{
	static class AzureConfigurationExtensions
	{
		public static TokenCredential ToTokenCredential(this IdentitySourceConfigAadApp config)
		{
			IPublicClientApplication app = PublicClientApplicationBuilder.Create(config.ApplicationID)
							.WithRedirectUri(config.RedirectURI)
							.WithTenantId(config.TenantID)
							.Build();
			return DelegatedTokenCredential.Create((reqContext, cancellationToken) =>
			{
				var result = app.AcquireTokenInteractive(reqContext.Scopes).ExecuteAsync().Result;
				return new AccessToken();
			},
			async (reqContext, cancellationToken) =>
			{
				var result = await app.AcquireTokenInteractive(reqContext.Scopes).ExecuteAsync();
				return new AccessToken(result.AccessToken, result.ExpiresOn);
			});
		}
	}
}
