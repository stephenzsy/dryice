using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.ConfigData
{
	internal class CosmosConfigDataProvider : IConfigDataProvider
	{
		private readonly CosmosClient client;
		private readonly string databaseId;

		public CosmosConfigDataProvider(CosmosClientBuilder clientBuilder, string databaseId) {
			client = clientBuilder.Build();
			this.databaseId = databaseId;
		}

		public async Task CheckSchemaAsync()
		{
			var db = client.GetDatabase(databaseId);
			await db.CreateContainerIfNotExistsAsync(new ContainerProperties("keyStores", "/type"));
			await db.CreateContainerIfNotExistsAsync(new ContainerProperties("keys", "/type"));
		}
	}
}
