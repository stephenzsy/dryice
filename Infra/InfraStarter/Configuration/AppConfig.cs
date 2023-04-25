using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InfraStarter.Configuration
{

	public record AppConfigJsonFile()
	{
		[JsonPropertyName("secrets")]
		public IList<SecretConfig> Secrets { get; set; }

		[JsonPropertyName("resources")]
		public IList<ResourceConfig> Resources { get; set; }
	}

	public class AppConfig
	{
		private static string ConfigFilePath => Path.Join(FileSystem.AppDataDirectory, "config.json");

		public AppConfig()
		{
			Resources = new Dictionary<Guid, ResourceConfig>();
		}

		public AppConfig(AppConfigJsonFile fileObject)
		{
			Resources = fileObject.Resources.ToDictionary((x) => x.ID);
		}

		public IDictionary<Guid, ResourceConfig> Resources { get; private set; }

		public static AppConfig LoadFromAppData()
		{
			try
			{
				return new AppConfig(JsonSerializer.Deserialize<AppConfigJsonFile>(File.ReadAllText(ConfigFilePath)));
			}
			catch (Exception)
			{
				return new AppConfig();
			}
		}

		public void SaveResource(ResourceConfig collected)
		{
			Resources[collected.ID] = collected;
			StoreFile();
			AppConfigChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Resources)));
		}

		private void StoreFile()
		{
			File.WriteAllText(ConfigFilePath, JsonSerializer.Serialize(new AppConfigJsonFile
			{
				Resources = Resources.Values.ToList(),
			}, options: new JsonSerializerOptions { WriteIndented = true }));
		}

		public event PropertyChangedEventHandler AppConfigChanged;
	}

}
