using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.ConfigData
{
	internal interface IConfigDataProvider
	{
		Task CheckSchemaAsync();
	}
}
