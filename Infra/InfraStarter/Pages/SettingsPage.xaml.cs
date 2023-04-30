using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfraStarter.Services;
using InfraStarter.Services.Models;
using InfraStarter.Services.Profile;
using System.Collections.ObjectModel;

namespace InfraStarter.Pages;

public partial class ActiveProfileViewModel : ObservableObject
{
	private readonly IActiveProfile profile;
	private readonly IProfileService profileService;
	public ActiveProfileViewModel(IActiveProfile activeProfile, IProfileService profileService)
	{
		this.profile = activeProfile;
		this.profileService = profileService;
	}

	public string Name => profile.Name;

	[RelayCommand]
	public Task CheckConfigDB()
	{
		return profile.CheckConfigDB();
	}

	[RelayCommand]
	public void Deactivate()
	{
		profile.Deactivate();
		profileService.ActivateProfile(Guid.Empty);
	}
}

public partial class SettingsPageViewModel : ObservableObject
{
	private readonly IProfileService profileService;

	public SettingsPageViewModel(IProfileService profileService)
	{
		this.profileService = profileService;
		OnProfilesChanged(null, profileService.ProfileConfigs);
		profileService.ProfilesChanged += OnProfilesChanged;
		OnSetActiveProfile(null, profileService.ActiveProfile);
		profileService.ActiveProfileChanged += OnSetActiveProfile;
	}

	~SettingsPageViewModel()
	{
		profileService.ProfilesChanged -= OnProfilesChanged;
	}

	[ObservableProperty]
	public IList<ProfileConfig> restProfiles;

	[RelayCommand]
	public Task AddProfile()
	{
		return Shell.Current.GoToAsync(nameof(ProfileDetailsPage), new Dictionary<string, object> {
			{ ProfileDetailsPage.ParamID, Guid.NewGuid() },
		});
	}

	private void OnSetActiveProfile(object sender, IActiveProfile activeProfile)
	{
		var v = profileService.ActiveProfile;
		if (v != null)
		{
			ActiveProfile = new ActiveProfileViewModel(v, profileService);
		}
		else
		{
			ActiveProfile = null;
		}
		OnProfilesChanged(sender, profileService.ProfileConfigs);
	}

	private void OnProfilesChanged(object sender, IList<ProfileConfig> e)
	{
		var allProfiles = e;
		RestProfiles = e.Where(x => x.ID != profileService.ActiveProfile?.ID).ToList().AsReadOnly();
	}

	[RelayCommand]
	public Task ViewProfile(Guid id)
	{
		return Shell.Current.GoToAsync(nameof(ProfileDetailsPage), new Dictionary<string, object> {
			{ ProfileDetailsPage.ParamID, id },
		});
	}

	[ObservableProperty]
	public ActiveProfileViewModel activeProfile;

	[RelayCommand]
	public void ActivateProfile(Guid id)
	{
		profileService.ActivateProfile(id);
	}
}

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}