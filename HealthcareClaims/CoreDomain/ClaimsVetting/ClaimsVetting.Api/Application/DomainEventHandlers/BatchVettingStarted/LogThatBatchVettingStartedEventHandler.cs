using System.Threading;
using System.Threading.Tasks;
using ClaimsVetting.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsVetting.Api.Application.DomainEventHandlers.BatchVettingStarted
{
	public class LogThatBatchVettingStartedEventHandler : INotificationHandler<BatchVettingStartedEvent>
	{
		private readonly ILogger<LogThatBatchVettingStartedEventHandler> _logger;

		public LogThatBatchVettingStartedEventHandler(
			ILogger<LogThatBatchVettingStartedEventHandler> logger)
		{
			_logger = logger;
		}

		public Task Handle(BatchVettingStartedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation(
				"Batch vetting started for batch {BatchId}, {BatchUri}",
				notification.BatchId,
				notification.BatchUri);

			return Task.CompletedTask;
		}
	}
}
