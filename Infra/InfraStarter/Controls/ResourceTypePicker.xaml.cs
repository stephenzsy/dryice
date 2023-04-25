using CommunityToolkit.Mvvm.ComponentModel;
using InfraStarter.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace InfraStarter.Controls;

public partial class ResourceTypePickerViewModel : ObservableObject
{

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(AvailableResourceTypes))]
	private string category;

	[ObservableProperty]
	private string resourceType;

	private static readonly ReadOnlyDictionary<string, ReadOnlyCollection<string>> ResourceCategoryMapping =
		new Dictionary<string, ReadOnlyCollection<string>>
		{
			{ ResourceCategoryName.IdentitySource, new List<string>{
				ResourceTypeName.AadApp
			}.AsReadOnly() },

			{ ResourceCategoryName.KeyProvider, new List<string>{
				ResourceTypeName.AzureKeyVault,
				ResourceTypeName.YubiKey
			}.AsReadOnly() }
		}.AsReadOnly();

	public IList<string> AvailableResourceTypes
	{
		get
		{
			if (Category == null)
			{
				return null;
			}
			return ResourceCategoryMapping[Category];
		}
	}

}

public partial class ResourceTypePicker : ContentView
{
	public static readonly BindableProperty ResourceTypeProperty = BindableProperty.Create(nameof(ResourceType),
		typeof(string),
		typeof(ResourceTypePicker),
		defaultValue: default(string),
		defaultBindingMode: BindingMode.OneWayToSource);

	public ResourceTypePickerViewModel ViewModel { get; init; }

	public ResourceTypePicker()
	{
		InitializeComponent();
		ViewModel = new ResourceTypePickerViewModel();
		Content.BindingContext = ViewModel;
		ViewModel.PropertyChanged += OnViewModelPropertyChanged;
	}

	~ResourceTypePicker()
	{
		ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
	}

	private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(ResourceTypePickerViewModel.ResourceType))
		{
			ResourceType = ViewModel.ResourceType;
		}
	}

	public string ResourceType
	{
		get
		{
			return ViewModel.ResourceType;
		}
		set
		{
			SetValue(ResourceTypeProperty, value);
		}
	}

}