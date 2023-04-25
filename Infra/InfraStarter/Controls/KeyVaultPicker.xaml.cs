using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfraStarter.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Resources;

namespace InfraStarter.Controls;

public record AzureSubscriptionResourceViewModel(SubscriptionResource SubscriptionResource)
{
	public string DisplayName => SubscriptionResource.Data.DisplayName;
}

public partial class KeyVaultPickerViewModel : ObservableObject
{

	private ArmClient armClient;
	private readonly AppConfig appConfig;

	public KeyVaultPickerViewModel(AppConfig appConfig)
	{
		this.appConfig = appConfig;

	}

	[ObservableProperty]
	private Guid identitySourceID;

	partial void OnIdentitySourceIDChanged(Guid value)
	{
		if (value.Equals(Guid.Empty))
		{
			Subscriptions.Clear();
			armClient = null;
			return;
		}

		var aadApp = appConfig.Resources[IdentitySourceID] as IdentitySourceConfigAadApp;
		armClient = new ArmClient(aadApp.ToTokenCredential());
	}

	[RelayCommand]
	public async Task GetSubscriptions()
	{
		if (armClient == null)
		{
			return;
		}

		Subscriptions.Clear();
		var subscriptions = armClient.GetSubscriptions()
			.GetAllAsync();
		await foreach (var p in subscriptions)
		{
			Subscriptions.Add(new AzureSubscriptionResourceViewModel(p));
		}
	}

	public ObservableCollection<AzureSubscriptionResourceViewModel> Subscriptions { get; } = new ObservableCollection<AzureSubscriptionResourceViewModel>();

	[ObservableProperty]
	private AzureSubscriptionResourceViewModel selectedSubscription;

	partial void OnSelectedSubscriptionChanged(AzureSubscriptionResourceViewModel value)
	{
		if(value != null)
		{
		//selectedSubscription.SubscriptionResource.GetResourceGroup
		}
	}
}

public partial class KeyVaultPicker : ContentView
{
	public static readonly BindableProperty IdentitySourceIDProperty = BindableProperty.Create(nameof(IdentitySourceID), typeof(Guid), typeof(KeyVaultPicker),
		defaultValue: Guid.Empty, propertyChanged: OnIdentitySourceIDChanged);

	private readonly KeyVaultPickerViewModel _viewModel;

	public KeyVaultPicker()
	{
		InitializeComponent();
		var currentApp = Application.Current as App;
		var vm = currentApp?.Services.GetService<KeyVaultPickerViewModel>();
		_viewModel = vm;
		MainView.BindingContext = vm;
	}

	public Guid IdentitySourceID
	{
		get
		{
			return (Guid)GetValue(IdentitySourceIDProperty);
		}
		set
		{
			SetValue(IdentitySourceIDProperty, value);
			_viewModel.IdentitySourceID = value;
		}
	}

	private static void OnIdentitySourceIDChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var picker = (KeyVaultPicker)bindable;
		picker.IdentitySourceID = (Guid)newValue;
	}
}