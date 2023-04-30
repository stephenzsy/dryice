using InfraStarter.Services.Models;
using Microsoft.Identity.Client;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.UI.WebUI;

namespace InfraStarter.Services.Profile
{
	public record ProfileLinkedAad
	{
		[JsonPropertyName("application-id")]
		public required string ApplicationID { get; init; }

		[JsonPropertyName("redirect-uri")]
		public required string RedirectURI { get; init; }

		[JsonPropertyName("tenant-id")]
		public string TenantID { get; init; }
	}

	public record ProfileCosmosConfig
	{
		[JsonPropertyName("account-endpoint")]
		public required string AccountEndpoint { get; init; }
	}


	public record ProfileConfig
	{
		[JsonPropertyName("id")]
		public required Guid ID { get; init; }

		[JsonPropertyName("name")]
		public required string Name { get; init; }

		[JsonPropertyName("linked-aad")]
		public required ProfileLinkedAad LinkedAad { get; init; }

		[JsonPropertyName("cosmos-config")]
		public required ProfileCosmosConfig CosmosConfig { get; init; }

		[JsonPropertyName("created-at")]
		public DateTimeOffset CreatedAt { get; set; }

		[JsonPropertyName("updated-at")]
		public DateTimeOffset UpdatedAt { get; set; }

		[JsonIgnore]
		public DateTime UpdatedAtLocal => UpdatedAt.LocalDateTime;
	}


	internal class ActiveProfile : IActiveProfile, ITokenProvider
	{

		private readonly ProfileConfig ProfileConfig;

		public ActiveProfile(ProfileConfig config)
		{
			ProfileConfig = config;
		}

		private IPublicClientApplication msalApp;

		public Guid ID => ProfileConfig.ID;
		public string Name => ProfileConfig.Name;

		private string ProfileInfoCacheFile => Path.Join(FileSystem.CacheDirectory, $"profile-{ID}.json");

		private IAccount cachedAccount;

		private bool deactivated = false;

		internal void Deactivate()
		{
			deactivated = false;
		}


		public async Task<IAccount> GetSignedInAccountAsync()
		{
			throw new NotImplementedException();
		}

		private static readonly ReadOnlyCollection<string> UserScopes = new List<string> { "https://graph.microsoft.com/User.Read" }.AsReadOnly();

		public async Task<IToken> AcuqireTokenAsync(ResourceScopeType scope)
		{
			if (deactivated) throw new ProfileDeactivatedException(ID);
			// load token from cached
			// load cached account
			IEnumerable<string> scopes;
			switch (scope)
			{
				case ResourceScopeType.User:
					scopes = UserScopes;
					break;
				default:
					scopes = Enumerable.Empty<string>();
					break;
			}
			var result = await MsalApp.AcquireTokenInteractive(scopes).ExecuteAsync();
			if (deactivated) throw new ProfileDeactivatedException(ID);
			// store token account to cache
			return new ResourceToken
			{
				AccessToken = result.AccessToken,
				ExpiresOn = result.ExpiresOn,
				Account = result.Account,
			};
		}
		/*
		private ResourceToken LoadResourceTokenFromCache()
		{

		}
		*/

		private IPublicClientApplication MsalApp
		{
			get
			{
				if (msalApp == null)
				{
					msalApp = PublicClientApplicationBuilder
						.Create(ProfileConfig.LinkedAad.ApplicationID)
						.WithRedirectUri(ProfileConfig.LinkedAad.RedirectURI)
						.WithTenantId(ProfileConfig.LinkedAad.TenantID)
						.Build();
				}
				return msalApp;
			}
		}
	}

}
