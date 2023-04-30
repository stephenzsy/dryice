using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.Profile
{
	internal class ProfileTokenProvider
	{
		private readonly IProfileService profileService;

		public ProfileTokenProvider(IProfileService profileService)
		{
			this.profileService = profileService;
		}
	}
}
