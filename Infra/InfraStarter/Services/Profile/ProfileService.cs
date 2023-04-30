using InfraStarter.Configuration;
using InfraStarter.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InfraStarter.Services.Profile
{
	internal record ProfilesConfigsFile
	{
		[JsonPropertyName("profiles")]
		public required IList<ProfileConfig> Profiles { get; init; }
	}

	internal class ProfileService : IProfileService
	{
		private static string ConfigFilePath => Path.Join(FileSystem.AppDataDirectory, "config.json");

		private ProfilesConfigsFile storedFile;

		public ProfileService()
		{
			try
			{
				storedFile = JsonSerializer.Deserialize<ProfilesConfigsFile>(File.ReadAllText(ConfigFilePath));
			}
			catch (Exception)
			{
				storedFile = new ProfilesConfigsFile { Profiles = new List<ProfileConfig>() };
			}

			var activeProfileIdStr = Preferences.Default.Get<string>("activeProfileId", string.Empty);
			if (!string.IsNullOrWhiteSpace(activeProfileIdStr))
			{
				if (Guid.TryParse(activeProfileIdStr, out Guid activeProfileId))
				{
					ActivateProfile(activeProfileId);
				}
			}
		}

		public IList<ProfileConfig> ProfileConfigs => storedFile.Profiles;

		private ActiveProfile activeProfile = null;
		public IActiveProfile ActiveProfile => activeProfile;

		public event EventHandler<IActiveProfile> ActiveProfileChanged;
		public event EventHandler<IList<ProfileConfig>> ProfilesChanged;


		public void SaveProfile(ProfileConfig config)
		{
			var existing = ProfileConfigs.Where(x => x.ID.Equals(config.ID)).FirstOrDefault();
			var nextProfileConfigs = ProfileConfigs.Where(x => !x.ID.Equals(config.ID)).ToList();
			config.UpdatedAt = DateTime.UtcNow;
			if (existing == null)
			{
				config.CreatedAt = config.UpdatedAt;
			}
			else
			{
				existing.CreatedAt = existing.CreatedAt;
			}

			nextProfileConfigs.Add(config);
			var nextFile = new ProfilesConfigsFile { Profiles = nextProfileConfigs };
			File.WriteAllText(ConfigFilePath, JsonSerializer.Serialize(new ProfilesConfigsFile { Profiles = nextProfileConfigs }, new JsonSerializerOptions
			{
				WriteIndented = true,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
			}));
			storedFile = nextFile;
			ProfilesChanged?.Invoke(this, ProfileConfigs);
			if (activeProfile?.ID == config.ID)
			{
				ActivateProfile(config.ID, true);
			}
		}

		public void ActivateProfile(Guid id, bool forced = false)
		{
			if (activeProfile?.ID != id || forced)
			{
				activeProfile?.Deactivate();
				activeProfile = null;
				var profileConfig = ProfileConfigs.Where(x => x.ID == id).FirstOrDefault();
				if (profileConfig != null)
				{
					activeProfile = new ActiveProfile(profileConfig);
					Preferences.Default.Set<string>("activeProfileId", profileConfig.ID.ToString());
				}
				ActiveProfileChanged?.Invoke(this, ActiveProfile);
			}
		}

		public void ActivateProfile(Guid id)
		{
			ActivateProfile(id, false);
		}
	}
}
