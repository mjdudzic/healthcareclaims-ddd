using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClaimsVetting.Api.Application.BackgroundJobs;
using ClaimsVetting.Api.Application.IntegrationEvents.Events;
using Hangfire;
using Infrastructure.Common.Web.Jaeger;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;

namespace ClaimsVetting.Api.Application.IntegrationEvents.EventHandlers
{
	public class BatchSubmissionCompletedEventHandler : INotificationHandler<BatchSubmissionCompletedEvent>
	{
		private readonly ITracer _tracer;
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly ILogger<BatchSubmissionCompletedEventHandler> _logger;

		public BatchSubmissionCompletedEventHandler(
			ITracer tracer,
			IBackgroundJobClient backgroundJobClient,
			ILogger<BatchSubmissionCompletedEventHandler> logger)
		{
			_tracer = tracer;
			_backgroundJobClient = backgroundJobClient;
			_logger = logger;
		}

		public Task Handle(BatchSubmissionCompletedEvent notification, CancellationToken cancellationToken)
		{
			using (var scope = TracingExtensions.StartServerSpan(
				_tracer,
				notification.TracingKeys,
				"batch-vetting-initiating"))
			{
				try
				{
					_logger.LogInformation(
						"Batch vetting initiated - URI: {BatchUri}",
						notification.BatchUri);

					var batchJobData = new BatchJobData
					{
						BatchUri = notification.BatchUri,
						TracingKeys = notification.TracingKeys
					};

					_backgroundJobClient
						.Enqueue<VetBatchJob>(
							job => job.Execute(batchJobData, null, JobCancellationToken.Null));
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
