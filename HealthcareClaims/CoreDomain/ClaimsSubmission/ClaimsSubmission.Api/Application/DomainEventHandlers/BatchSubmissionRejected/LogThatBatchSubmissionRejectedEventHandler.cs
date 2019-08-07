using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Api.Application.DomainEventHandlers.BatchSubmissionFeedbackGenerated;
using ClaimsSubmission.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsSubmission.Api.Application.DomainEventHandlers.BatchSubmissionRejected
{
	public class LogThatBatchSubmissionRejectedEventHandler : INotificationHandler<BatchSubmissionRejectedEvent>
	{
		private readonly ILogger<LogThatBatchSubmissionFeedbackGeneratedEventHandler> _logger;

		public LogThatBatchSubmissionRejectedEventHandler(
			ILogger<LogThatBatchSubmissionFeedbackGeneratedEventHandler> logger)
		{
			_logger = logger;
		}

		public Task Handle(BatchSubmissionRejectedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Batch submission rejected for batch {BatchId}", notification.BatchId);

			return Task.CompletedTask;
		}
	}
}
