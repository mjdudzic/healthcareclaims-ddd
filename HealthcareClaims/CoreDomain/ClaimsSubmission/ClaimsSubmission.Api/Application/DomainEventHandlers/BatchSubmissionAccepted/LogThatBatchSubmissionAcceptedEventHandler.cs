using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Api.Application.DomainEventHandlers.BatchSubmissionFeedbackGenerated;
using ClaimsSubmission.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsSubmission.Api.Application.DomainEventHandlers.BatchSubmissionAccepted
{
	public class LogThatBatchSubmissionAcceptedEventHandler : INotificationHandler<BatchSubmissionAcceptedEvent>
	{
		private readonly ILogger<LogThatBatchSubmissionFeedbackGeneratedEventHandler> _logger;

		public LogThatBatchSubmissionAcceptedEventHandler(
			ILogger<LogThatBatchSubmissionFeedbackGeneratedEventHandler> logger)
		{
			_logger = logger;
		}

		public Task Handle(BatchSubmissionAcceptedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Batch submission accepted for batch {BatchId}", notification.BatchId);

			return Task.CompletedTask;
		}
	}
}
