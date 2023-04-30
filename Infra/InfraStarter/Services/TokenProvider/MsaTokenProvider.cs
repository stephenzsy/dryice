using InfraStarter.Services.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.TokenProvider
{
	internal class MsaTokenProvider
	{
		private readonly IProfileService profileService;

		public MsaTokenProvider(IProfileService profileService)
		{
			this.profileService = profileService;
		}
	}
}
