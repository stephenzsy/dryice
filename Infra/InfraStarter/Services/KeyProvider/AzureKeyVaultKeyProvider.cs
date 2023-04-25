using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.KeyProvider
{
	internal sealed class AzureKeyVaultKeyProvider : IKeyProvider
	{
		public bool IsUnlocked => throw new NotImplementedException();

		public void Lock()
		{
			throw new NotImplementedException();
		}

		public Task<bool> TryUnlockAsync()
		{
			throw new NotImplementedException();
		}

	}
}
