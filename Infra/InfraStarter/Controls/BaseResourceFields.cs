using InfraStarter.Configuration;
using InfraStarter.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Controls
{
	public abstract class BaseResourceFields : ContentView
	{
		public ResourceFieldsViewModel ViewModel { get; protected set; }
	}
}
