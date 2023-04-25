using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InfraStarter.Configuration
{
	public static class SecretTypeName
	{
		public const string AzureKeyVaultSecret = "keyvault-secret";
	}

	[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
	[JsonDerivedType(typeof(AzureKeyVaultSecretConfig), typeDiscriminator: SecretTypeName.AzureKeyVaultSecret)]

	public abstract record SecretConfig([property: JsonPropertyName("id")] Guid ID)
	{
		[JsonPropertyName("name")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Name { get; set; }

		[JsonIgnore]
		public abstract string ResourceType { get; }

	}

	internal record AzureKeyVaultSecretConfig(
		Guid ID,
		[property: JsonPropertyName("keyvault-resource-id")] Guid KeyVaultResourceID) : SecretConfig(ID)
	{
		[JsonIgnore]
		public override string ResourceType => SecretTypeName.AzureKeyVaultSecret;
	}
}
