using ClaimsSubmission.Infrastructure.Queue.Interfaces;

namespace ClaimsSubmission.Infrastructure.Queue.Configuration
{
	public class RabbitMqTopologyConfiguration : IRabbitMqTopologyConfiguration
	{
		public string ExchangeName { get; set; }
		public string RoutingKey { get; set; }
		public string BatchesSubmittedQueueName { get; set; }
	}
}