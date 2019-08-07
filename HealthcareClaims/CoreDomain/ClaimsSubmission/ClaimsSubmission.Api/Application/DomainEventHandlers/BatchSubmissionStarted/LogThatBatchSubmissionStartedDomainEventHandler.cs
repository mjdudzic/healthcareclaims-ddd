using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ClaimsSubmission.Api.Application.DomainEventHandlers.BatchSubmissionStarted
{
	public class LogThatBatchSubmissionStartedDomainEventHandler : INotificationHandler<BatchSubmissionStartedEvent>
	{
		private readonly ILogger<LogThatBatchSubmissionStartedDomainEventHandler> _logger;

		public LogThatBatchSubmissionStartedDomainEventHandler(
			ILogger<LogThatBatchSubmissionStartedDomainEventHandler> logger)
		{
			_logger = logger;
		}

		public Task Handle(BatchSubmissionStartedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Batch submission started: {JsonConvert.SerializeObject(notification)}");

			return Task.CompletedTask;
		}
	}
}
