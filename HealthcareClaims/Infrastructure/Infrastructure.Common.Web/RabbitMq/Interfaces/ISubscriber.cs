using MediatR;

namespace Infrastructure.Common.Web.RabbitMq.Interfaces
{
	public interface IBusSubscribeService
	{
		void SubscribeForEvent<T>(
			string exchangeName,
			string routingKey,
			string queueName)
			where T : INotification;
	}
}