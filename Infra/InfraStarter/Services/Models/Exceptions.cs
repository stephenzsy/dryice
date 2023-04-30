using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.Models
{
	internal class ProfileDeactivatedException : Exception
	{
		public ProfileDeactivatedException(Guid profileId) : base($"Profile {profileId} has been deactivated") { }
	}
}
