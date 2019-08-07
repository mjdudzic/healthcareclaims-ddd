using System.Threading.Tasks;
using MediatR;

namespace Infrastructure.Common.Web.RabbitMq.Interfaces
{
	public interface IBusPublishService
	{
		Task PublishEvent<T>(
			T @event,
			string exchangeName,
			string routingKey)
			where T : INotification;
	}
}