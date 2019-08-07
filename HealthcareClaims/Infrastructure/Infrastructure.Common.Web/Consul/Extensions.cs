using System;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Common.Web.Consul
{
	public static class Extensions
	{
		public static void AddServiceDiscovery(this IServiceCollection services)
		{
			var serviceConfig = GetServiceConfigOptions(services);

			if (serviceConfig.Enabled == false)
			{
				return;
			}

			var consulClient = CreateConsulClient(serviceConfig);

			services.AddSingleton(serviceConfig);
			services.AddSingleton<IHostedService, ServiceDiscoveryHostedService>();
			services.AddSingleton<IConsulClient, ConsulClient>(p => consulClient);
		}

		private static ServiceConfig GetServiceConfigOptions(IServiceCollection services)
		{
			using (var serviceProvider = services.BuildServiceProvider())
			{
				var configuration = serviceProvider.GetService<IConfiguration>();
				var options = new ServiceConfig();
				configuration.Bind("ServiceConfig", options);
				options.ServiceId = Guid.NewGuid().ToString();

				return options;
			}
		}

		private static ConsulClient CreateConsulClient(ServiceConfig serviceConfig)
		{
			return new ConsulClient(config =>
			{
				config.Address = serviceConfig.ServiceDiscoveryAddress;
			});
		}
	}
}