using CommunityToolkit.Mvvm.ComponentModel;
using InfraStarter.Configuration;
using System.ComponentModel.DataAnnotations;

namespace InfraStarter.ViewModels.ResourceConfigForm
{
	public abstract partial class ResourceConfigFormViewModelBase : ObservableValidator
	{
		[ObservableProperty]
		[Required(ErrorMessage = "Name is required.")]
		private string name;


		private ResourceConfig originalData;

		public ResourceConfig OriginalData
		{
			get => originalData;
			set
			{
				originalData = value;
				ResetFromOriginalData(value);
			}
		}

		protected virtual void ResetFromOriginalData(ResourceConfig value)
		{
			if (value == null)
			{
				Name = null;
			}
			Name = value.Name;
		}

		public IList<ValidationResult> ValidateForm()
		{
			ValidateProperty(Name, nameof(Name));
			ValidateAllProperties();
			return HasErrors ? GetErrors().ToList() : null;
		}

		public abstract ResourceConfig GetResourceConfig();
	}
}
