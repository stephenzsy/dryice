using InfraStarter.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.KeyProvider
{
	public interface IKeyProvider
	{
		public bool IsUnlocked { get; }

		public Task<bool> TryUnlockAsync();

		public void Lock();
	}

	public interface IKeyProviderService
	{
		public T GetKeyProvider<T>(Guid configId) where T : IKeyProvider;
	}
}
