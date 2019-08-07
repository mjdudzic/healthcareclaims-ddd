using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClaimsVetting.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Propagation;

namespace ClaimsVetting.Api.Application.DomainEventHandlers.BatchVettingCompleted
{
	public class LogThatBatchVettingCompletedEventHandler : INotificationHandler<BatchVettingCompletedEvent>
	{
		private readonly ITracer _tracer;
		private readonly ILogger<LogThatBatchVettingCompletedEventHandler> _logger;

		public LogThatBatchVettingCompletedEventHandler(
			ITracer tracer,
			ILogger<LogThatBatchVettingCompletedEventHandler> logger)
		{
			_tracer = tracer;
			_logger = logger;
		}

		public Task Handle(BatchVettingCompletedEvent notification, CancellationToken cancellationToken)
		{
			var activeSpan = _tracer.ActiveSpan;
			var dictionary = new Dictionary<string, string>();

			if (activeSpan != null)
			{
				_tracer.Inject(activeSpan.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(dictionary));
			}

			_logger.LogInformation(
				"Batch vetting completed for batch {BatchId}, {BatchUri} - report URI: {VettingReportUri}", 
				notification.BatchId, 
				notification.BatchUri,
				notification.VettingReportUri);

			return Task.CompletedTask;
		}
	}
}
