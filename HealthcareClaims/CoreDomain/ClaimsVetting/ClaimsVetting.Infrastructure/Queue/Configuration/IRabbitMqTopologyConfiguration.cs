namespace ClaimsVetting.Infrastructure.Queue.Configuration
{
	public interface IRabbitMqTopologyConfiguration
	{
		string ExchangeName { get; set; }
		string RoutingKey { get; set; }
		string BatchesSubmittedQueueName { get; set; }
	}
}