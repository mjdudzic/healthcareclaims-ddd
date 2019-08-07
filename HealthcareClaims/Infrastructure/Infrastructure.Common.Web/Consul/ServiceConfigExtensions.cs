using System;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Common.Web.Consul
{
	public static class ServiceConfigExtensions
	{
		public static ServiceConfig GetServiceConfig(this IConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			var serviceConfig = new ServiceConfig();
			configuration.Bind("ServiceConfig", serviceConfig);
			serviceConfig.ServiceId = Guid.NewGuid().ToString();

			return serviceConfig;
		}
	}
}