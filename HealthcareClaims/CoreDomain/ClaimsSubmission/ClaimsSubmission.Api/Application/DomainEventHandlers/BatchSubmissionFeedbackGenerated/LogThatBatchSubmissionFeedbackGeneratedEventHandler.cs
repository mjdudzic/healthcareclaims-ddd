using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClaimsSubmission.Api.Application.DomainEventHandlers.BatchSubmissionFeedbackGenerated
{
	public class LogThatBatchSubmissionFeedbackGeneratedEventHandler : INotificationHandler<BatchSubmissionFeedbackGeneratedEvent>
	{
		private readonly ILogger<LogThatBatchSubmissionFeedbackGeneratedEventHandler> _logger;

		public LogThatBatchSubmissionFeedbackGeneratedEventHandler(
			ILogger<LogThatBatchSubmissionFeedbackGeneratedEventHandler> logger)
		{
			_logger = logger;
		}

		public Task Handle(BatchSubmissionFeedbackGeneratedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation(
				"Batch submission feedback for batch {BatchId} generated - URI: {BatchFeedbackUri}", 
				notification.BatchId,
				notification.BatchFeedbackUri);

			return Task.CompletedTask;
		}
	}
}
