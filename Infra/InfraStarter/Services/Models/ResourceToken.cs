using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InfraStarter.Services.Models
{
	internal record ResourceToken : IToken
	{
		public required IAccount Account { get; init; }

		public required string AccessToken { get; init; }

		public required DateTimeOffset ExpiresOn { get; init; }
	}

	internal record AccountRecord : IAccount
	{
		public AccountRecord() { }

		public AccountRecord(IAccount account)
		{
			Username = account.Username;
			Environment = account.Environment;
			homeAccountId = account.HomeAccountId;
			Identifier = account.HomeAccountId.Identifier;
			ObjectId = account.HomeAccountId.ObjectId;
			TenantId = account.HomeAccountId.TenantId;
		}

		[JsonPropertyName("username")]
		public required string Username { get; init; }

		[JsonPropertyName("environment")]
		public required string Environment { get; init; }

		[JsonPropertyName("identifier")]
		public required string Identifier { get; init; }

		[JsonPropertyName("object-id")]
		public required string ObjectId { get; init; }

		[JsonPropertyName("tenant-id")]
		public required string TenantId { get; init; }

		private AccountId homeAccountId = null;

		[JsonIgnore]
		public AccountId HomeAccountId => homeAccountId ??= new AccountId(Identifier, ObjectId, TenantId);
	}

}
