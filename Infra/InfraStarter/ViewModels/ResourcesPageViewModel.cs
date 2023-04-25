using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfraStarter.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraStarter.ViewModels
{

	public class ResourceConfigGroupViewModel : List<ResourceConfigSummaryViewModel>
	{
		public ResourceConfigGroupViewModel(IGrouping<string, ResourceConfig> grouping) : base(grouping.Select(x => new ResourceConfigSummaryViewModel(x)))
		{
			GroupName = grouping.Key;
			ResourceConfigs = grouping.ToList();
		}

		public string GroupName { get; init; }

		public List<ResourceConfig> ResourceConfigs { get; init; }

	}

	public partial class ResourcesPageViewModel : ObservableObject
	{
		[ObservableProperty]
		private ReadOnlyCollection<ResourceConfigGroupViewModel> resourceConfigGroups;

		private readonly AppConfig appConfig;

		public ResourcesPageViewModel(AppConfig appConfig)
		{
			this.appConfig = appConfig;
			ResourceConfigGroups = new ReadOnlyCollection<ResourceConfigGroupViewModel>(
					appConfig.Resources.Values
					.GroupBy(x => x.Category)
					.Select(x => new ResourceConfigGroupViewModel(x))
					.ToList()
				);
			appConfig.AppConfigChanged += onAppConfigChanged;
		}

		private void onAppConfigChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == nameof(AppConfig.Resources))
			{
				ResourceConfigGroups = new ReadOnlyCollection<ResourceConfigGroupViewModel>(
					appConfig.Resources.Values
					.GroupBy(x => x.Category)
					.Select(x => new ResourceConfigGroupViewModel(x))
					.ToList()
				);
			}
		}

		[RelayCommand]
		public Task CreateResource()
		{
			return Shell.Current.GoToAsync(nameof(ResourceDetailsPage), new Dictionary<string, object> {
				{ResourceDetailsPage.ParamID, Guid.Empty }
			});
		}

		[RelayCommand]
		public Task ViewResource(Guid ID)
		{
			return Shell.Current.GoToAsync(nameof(ResourceDetailsPage), new Dictionary<string, object> {
				{ResourceDetailsPage.ParamID, ID }
			});
		}
	}
}
