using System.Text.Json.Serialization;

namespace InfraStarter.Configuration
{
	static class ResourceCategoryName
	{
		public const string Unknown = "unknown";
		public const string IdentitySource = "identity-source";
		public const string KeyProvider = "key-provider";
	}

	static class ResourceTypeName
	{
		public const string AadApp = "aad-app";
		public const string AzureKeyVault = "azure-keyvault";
		public const string YubiKey = "yubikey";
	}

	static class ResourceSecureStorageKey
	{
		public const string AadAppGraphToken = "aad-app-graph-token";
		public static string GetAadAppKey(Guid ResourceID, string tokenType)
		{
			return $"{ResourceID}:${tokenType}";
		}
	}

	public record ResourceConfigCommon(
	[property: JsonPropertyName("id")] Guid ID,
	[property: JsonPropertyName("name")] string Name)
	{
	}

	[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
	[JsonDerivedType(typeof(IdentitySourceConfigAadApp), typeDiscriminator: ResourceTypeName.AadApp)]
	[JsonDerivedType(typeof(KeyProviderConfigAzureKeyVault), typeDiscriminator: ResourceTypeName.AzureKeyVault)]
	[JsonDerivedType(typeof(KeyProviderConfigYubiKey), typeDiscriminator: ResourceTypeName.YubiKey)]
	public abstract record ResourceConfig(
		Guid ID,
		string Name) : ResourceConfigCommon(ID, Name)
	{
		public abstract string ResourceType { get; }
		internal virtual string Category => ResourceCategoryName.Unknown;
	}

	public record IdentitySourceConfigAadApp(
		Guid ID,
		string Name,
		[property: JsonPropertyName("app-id")] string ApplicationID,
		[property: JsonPropertyName("redirect-uri")] string RedirectURI) : ResourceConfig(ID, Name)
	{

		[JsonIgnore]
		public override string ResourceType => ResourceTypeName.AadApp;

		internal override string Category => ResourceCategoryName.IdentitySource;

		[JsonPropertyName("tenant-id")]
		public string TenantID { get; init; }
	}

	public record KeyProviderConfigAzureKeyVault(
		Guid ID,
		string Name,
		[property: JsonPropertyName("keyvault-uri")] string KeyVaultURI,
		[property: JsonPropertyName("identity-source-id")] Guid IdentitySourceID) : ResourceConfig(ID, Name)
	{

		[JsonIgnore]
		public override string ResourceType => ResourceTypeName.AzureKeyVault;

		internal override string Category => ResourceCategoryName.KeyProvider;
	}


	public record KeyProviderConfigYubiKey(
		Guid ID,
		string Name) : ResourceConfig(ID, Name)
	{

		[JsonIgnore]
		public override string ResourceType => ResourceTypeName.YubiKey;

		internal override string Category => ResourceCategoryName.KeyProvider;

	}
}
