using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfraStarter.Services;
using InfraStarter.Services.Profile;
using System.ComponentModel.DataAnnotations;

namespace InfraStarter.Pages;

public partial class ProfileDetailsPageViewModel : ObservableValidator
{

	public ProfileDetailsPageViewModel(IProfileService profileService)
	{
		this.profileService = profileService;
	}

	public Guid ProfileID { get; private set; }


	[ObservableProperty]
	[Required]
	private string profileName;
	[ObservableProperty]
	[Required]
	private string aadAppId;
	[ObservableProperty]
	[Required]
	private string redirectUri;
	[ObservableProperty]
	[Required]
	private string tenantId;
	[ObservableProperty]
	[Required]
	private string cosmosAccountEndpoint;
	[ObservableProperty]
	[Required]
	private string databaseId;
	[ObservableProperty]
	private string accountPrimaryKey;

	private readonly IProfileService profileService;

	public void ResetToProfileID(Guid ID)
	{
		ProfileID = ID;
		bool isNew = true;
		if (!profileService.ProfileConfigs.Any())
		{
			ProfileName = "Default";
		}
		else
		{
			var selectedProfile = profileService.ProfileConfigs.Where(x => ID.Equals(x.ID)).FirstOrDefault();
			if (selectedProfile != null)
			{
				isNew = false;
				ProfileName = selectedProfile.Name;
				AadAppId = selectedProfile.LinkedAad.ApplicationID;
				RedirectUri = selectedProfile.LinkedAad.RedirectURI;
				TenantId = selectedProfile.LinkedAad.TenantID;
				CosmosAccountEndpoint = selectedProfile.CosmosConfig.AccountEndpoint;
				DatabaseId = selectedProfile.CosmosConfig.DatabaseId;
				AccountPrimaryKey = selectedProfile.CosmosConfig.DevAccountKey;
			}
			else
			{
				ProfileName = $"Profile-{ID}";
			}
		}
		if (isNew)
		{
			AadAppId = null;
			RedirectUri = null;
			TenantId = null;
			CosmosAccountEndpoint = null;
			DatabaseId = null;
			AccountPrimaryKey = null;
		}
		DisplayValidationErrors = null;
	}

	[ObservableProperty]
	private IList<ValidationResult> displayValidationErrors;

	[RelayCommand]
	public void Save()
	{
		ValidateAllProperties();
		if (HasErrors)
		{
			DisplayValidationErrors = GetErrors().ToList();
			return;
		}

		var config = new ProfileConfig
		{
			ID = ProfileID,
			Name = ProfileName,
			LinkedAad = new ProfileLinkedAad { ApplicationID = AadAppId, RedirectURI = RedirectUri, TenantID = TenantId },
			CosmosConfig = new ProfileCosmosConfig { AccountEndpoint = CosmosAccountEndpoint, DatabaseId = DatabaseId, DevAccountKey = AccountPrimaryKey },
		};
		profileService.SaveProfile(config);
	}

	[RelayCommand]
	public void Cancel()
	{
		Shell.Current.SendBackButtonPressed();
	}
}

[QueryProperty(nameof(ID), ParamID)]
public partial class ProfileDetailsPage : ContentPage
{
	private readonly ProfileDetailsPageViewModel viewModel;
	public ProfileDetailsPage(ProfileDetailsPageViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
		BindingContext = vm;
	}

	public Guid ID
	{
		set
		{
			viewModel.ResetToProfileID(value);
		}
	}

	public const string ParamID = "id";
}