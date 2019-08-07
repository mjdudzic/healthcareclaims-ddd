using Infrastructure.Common.Web.RabbitMq.Interfaces;
using MediatR;
using RawRabbit;
using RawRabbit.Configuration.Exchange;

namespace Infrastructure.Common.Web.RabbitMq
{
	public class BusSubscribeService : IBusSubscribeService
	{
		private readonly IBusClient _busClient;
		private readonly IMediator _mediator;

		public BusSubscribeService(
			IBusClient busClient,
			IMediator mediator)
		{
			_busClient = busClient;
			_mediator = mediator;
		}

		public void SubscribeForEvent<T>(
			string exchangeName,
			string routingKey,
			string queueName)
			where T: INotification
		{
			_busClient.SubscribeAsync<T>(
				async msg =>
				{
					await _mediator.Publish(msg);
				},
				ctx => ctx
					.UseSubscribeConfiguration(cfg => cfg
						.Consume(c => c
							.WithRoutingKey(routingKey))
						.FromDeclaredQueue(q => q
							.WithName(queueName)
							.WithDurability()
							.WithAutoDelete(false))
						.OnDeclaredExchange(e => e
							.WithName(exchangeName)
							.WithType(ExchangeType.Topic)))
				);
		}
	}
}