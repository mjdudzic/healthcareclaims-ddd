using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;

namespace Identity.Api.Infrastructure.Persistence
{
	public class DataSeedService
	{
		private readonly IServiceProvider _services;

		public DataSeedService(IServiceProvider services)
		{
			_services = services;
		}

		public async Task SeedClients()
		{
			await AddClient(
				StartupClientData.CreateTestClient(
					"hpc-client", 
					"HPC Client 1"));

			await AddClient(
				StartupClientData.CreateTestClient(
					"hpc-client-vip",
					"HPC Client VIP"),
				true);
		}

		private async Task AddClient(StartupClientData client, bool isVip = false)
		{
			using (var scope = _services.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

				if (await manager.FindByClientIdAsync(client.ClientId) == null)
				{
					var descriptor = new OpenIddictApplicationDescriptor
					{
						ClientId = client.ClientId,
						ClientSecret = client.ClientSecret,
						DisplayName = client.DisplayName,
						Permissions =
						{
							//OpenIddictConstants.Permissions.Endpoints.Authorization,
							OpenIddictConstants.Permissions.Endpoints.Token,
							//OpenIddictConstants.Permissions.Endpoints.Logout,
							OpenIddictConstants.Permissions.Scopes.Email,
							OpenIddictConstants.Permissions.Scopes.Profile,
							OpenIddictConstants.Permissions.Scopes.Roles,
							OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
						}
					};

					if (isVip)
					{
						descriptor.Permissions.Add($"{OpenIddictConstants.Permissions.Prefixes.Scope}is_vip");
					}

					await manager.CreateAsync(descriptor);
				}
			}
		}

		internal class StartupClientData
		{
			public string ClientId { get; set; }
			public string ClientSecret { get; set; }
			public string DisplayName { get; set; }

			public static StartupClientData CreateTestClient(
				string clientId,
				string displayName)
			{
				return new StartupClientData
				{
					ClientId = clientId,
					ClientSecret = "test123",
					DisplayName = displayName
				};
			}
		}
	}
}