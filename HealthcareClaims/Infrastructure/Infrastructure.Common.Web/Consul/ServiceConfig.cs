using System;

namespace Infrastructure.Common.Web.Consul
{
	public class ServiceConfig
	{
		public bool Enabled { get; set; }
		public Uri ServiceDiscoveryAddress { get; set; }
		public Uri ServiceAddress { get; set; }
		public string ServiceName { get; set; }
		public string ServiceId { get; set; }
		public string HealthCheckEndpoint { get; set; }
	}
}
