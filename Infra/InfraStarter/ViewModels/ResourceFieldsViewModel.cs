using CommunityToolkit.Mvvm.ComponentModel;
using InfraStarter.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.ViewModels
{
	public abstract class ResourceFieldsViewModel : ObservableValidator
	{
		public abstract ResourceConfig CollectData(ResourceConfigCommon common);

		public IEnumerable<ValidationResult> ValidationErrors { get; protected set; }

		public abstract ResourceConfig OriginalData { set; }

	}
}
