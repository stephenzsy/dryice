using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.Models
{
	public interface IActiveProfile
	{
		public Guid ID { get; }
		public Task<IAccount> GetSignedInAccountAsync();

		public string Name { get; }
	}
}
