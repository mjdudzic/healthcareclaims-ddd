using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Api.Application.Messages.Events;
using Infrastructure.Common.Web.Jaeger;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;

namespace ClaimsSubmission.Api.Application.Messages.EventHandlers
{
	public class BatchSubmissionCompletedEventHandler : INotificationHandler<BatchSubmissionCompletedEvent>
	{
		private readonly ITracer _tracer;
		private readonly ILogger<BatchSubmissionCompletedEventHandler> _logger;

		public BatchSubmissionCompletedEventHandler(
			ITracer tracer,
			ILogger<BatchSubmissionCompletedEventHandler> logger)
		{
			_tracer = tracer;
			_logger = logger;
		}

		public Task Handle(BatchSubmissionCompletedEvent notification, CancellationToken cancellationToken)
		{
			using (var scope = TracingExtensions.StartServerSpan(
				_tracer,
				notification.TracingKeys,
				"batch-submission-completion"))
			{
				try
				{
					_logger.LogInformation(
						"Batch submission completion handled - URI: {BatchUri}",
						notification.BatchUri);
				}
				catch (Exception e)
				{
					scope.Span.Log(new Dictionary<string, object>() { { "exception", e } });
					Tags.Error.Set(scope.Span, true);
				}

				return Task.CompletedTask;
			}
		}
	}
}
