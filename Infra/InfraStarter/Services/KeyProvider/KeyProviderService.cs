using InfraStarter.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.KeyProvider
{
	internal class KeyProviderService : IKeyProviderService
	{
		private readonly AppConfig AppConfig;
		public KeyProviderService(AppConfig appConfig)
		{
			AppConfig = appConfig;
		}

		public T GetKeyProvider<T>(Guid configId) where T : IKeyProvider
		{
			var config = AppConfig.Resources[configId];
			if (config == null || config.Category != ResourceCategoryName.KeyProvider)
			{
				throw new ArgumentException($"{configId} is not a valid key provider resource ID.");
			}
			throw new NotImplementedException();
		}
	}
}
