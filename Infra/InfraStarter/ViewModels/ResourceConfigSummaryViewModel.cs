using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfraStarter.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.ViewModels
{
	public record ResourceConfigSummaryViewModel
	{
		public ResourceConfigSummaryViewModel(ResourceConfig config)
		{
			Config = config;
		}

		private ResourceConfig Config { get; init; }

		public Guid ID => Config.ID;

		public string Name => Config.Name;

		public string ResourceType => Config.ResourceType;
	}
}
