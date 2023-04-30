using InfraStarter.Services.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services
{
	internal interface ITokenProvider
	{
		public Task<IToken> AcuqireTokenAsync(ResourceScopeType scope);
	}
}
