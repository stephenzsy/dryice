using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStarter.Services.Models
{
	internal interface IToken
	{
		IAccount Account { get; }
	}
}
