using Azure;
using Azure.Core;
using Azure.Security.KeyVault.Secrets;
using System.Security.Cryptography;

namespace InfraStarter.Services.KeyProvider
{
	internal sealed class AzureKeyVaultSecretsProvider
	{
		private readonly SecretClient SecretClient;
		public AzureKeyVaultSecretsProvider(Uri vaultUri, TokenCredential tokenCredential)
		{
			SecretClient = new SecretClient(vaultUri, tokenCredential);
		}

		public Task<Response<KeyVaultSecret>> GenerateRandomSecretAsync(string name, ref byte[] secretBytes, string contentType = default, CancellationToken cancellationToken = default)
		{
			RandomNumberGenerator.Fill(secretBytes);
			var s = new KeyVaultSecret(name, Convert.ToHexString(secretBytes));
			if (!string.IsNullOrWhiteSpace(contentType))
			{
				s.Properties.ContentType = contentType;
			}
			return SecretClient.SetSecretAsync(s, cancellationToken);
		}
	}
}
