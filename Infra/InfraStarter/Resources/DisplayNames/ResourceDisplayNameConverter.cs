using InfraStarter.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Resources.DisplayNames
{
	internal class ResourceDisplayNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string)
			{
				switch (value as string)
				{
					case ResourceCategoryName.IdentitySource:
						return ResourceDisplayNames.CategoryIdentitySource;
					case ResourceCategoryName.KeyProvider:
						return ResourceDisplayNames.CategoryKeyProvider; ;
					case ResourceTypeName.AadApp:
						return ResourceDisplayNames.TypeAadApp;
					case ResourceTypeName.AzureKeyVault:
						return ResourceDisplayNames.TypeAzureKeyVault;
					case ResourceTypeName.YubiKey:
						return ResourceDisplayNames.TypeYubiKey;
				}
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
