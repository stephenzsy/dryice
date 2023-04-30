using InfraStarter.Services.Models;
using InfraStarter.Services.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services
{
	public interface IProfileService
	{
		public IActiveProfile ActiveProfile { get; }
		IList<ProfileConfig> ProfileConfigs { get; }

		event EventHandler<IActiveProfile> ActiveProfileChanged;
		event EventHandler<IList<ProfileConfig>> ProfilesChanged;

		void SaveProfile(ProfileConfig config);
				
		void ActivateProfile(Guid id);
	}
}
