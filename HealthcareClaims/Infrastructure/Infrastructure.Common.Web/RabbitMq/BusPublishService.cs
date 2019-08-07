using System.Threading.Tasks;
using Infrastructure.Common.Web.RabbitMq.Interfaces;
using MediatR;
using RawRabbit;
using RawRabbit.Configuration.Exchange;

namespace Infrastructure.Common.Web.RabbitMq
{
	public class BusPublishService : IBusPublishService
	{
		private readonly IBusClient _busClient;

		public BusPublishService(
			IBusClient busClient)
		{
			_busClient = busClient;
		}

		public async Task PublishEvent<T>(
			T @event,
			string exchangeName,
			string routingKey)
			where T : INotification
		{
			await _busClient.PublishAsync(@event, ctx => ctx
				.UsePublishConfiguration(cfg => cfg
					.OnDeclaredExchange(e => e
						.WithName(exchangeName)
						.WithType(ExchangeType.Topic))
					.WithRoutingKey(routingKey)
				));
		}
	}
}