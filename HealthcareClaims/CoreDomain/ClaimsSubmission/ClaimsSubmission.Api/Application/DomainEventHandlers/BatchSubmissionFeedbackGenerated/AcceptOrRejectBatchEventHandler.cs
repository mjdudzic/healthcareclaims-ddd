using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Api.Application.Messages.Events;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using ClaimsSubmission.Domain.Events;
using ClaimsSubmission.Infrastructure.Queue.Interfaces;
using Infrastructure.Common.Web.RabbitMq.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Propagation;

namespace ClaimsSubmission.Api.Application.DomainEventHandlers.BatchSubmissionFeedbackGenerated
{
	public class AcceptOrRejectBatchEventHandler : INotificationHandler<BatchSubmissionFeedbackGeneratedEvent>
	{
		private readonly IBatchRepository _batchRepository;
		private readonly IBusPublishService _busPublishService;
		private readonly IRabbitMqTopologyConfiguration _rabbitMqTopologyConfiguration;
		private readonly ITracer _tracer;
		private readonly ILogger<AcceptOrRejectBatchEventHandler> _logger;

		public AcceptOrRejectBatchEventHandler(
			IBatchRepository batchRepository,
			IBusPublishService busPublishService,
			IRabbitMqTopologyConfiguration rabbitMqTopologyConfiguration,
			ITracer tracer,
			ILogger<AcceptOrRejectBatchEventHandler> logger)
		{
			_batchRepository = batchRepository;
			_busPublishService = busPublishService;
			_rabbitMqTopologyConfiguration = rabbitMqTopologyConfiguration;
			_tracer = tracer;
			_logger = logger;
		}

		public async Task Handle(BatchSubmissionFeedbackGeneratedEvent notification, CancellationToken cancellationToken)
		{
			var batch = await _batchRepository.BatchForIdAsync(notification.BatchId);

			if (batch == null)
			{
				_logger.LogError($"Batch {notification.BatchId} not found");
				return;
			}

			var batchHasErrors = notification.HasErrors;

			if (batchHasErrors)
			{
				_logger.LogInformation($"Batch {notification.BatchId} is rejected");
				batch.Reject();
			}
			else
			{
				_logger.LogInformation($"Batch {notification.BatchId} is accepted");
				batch.Accept();

				var activeSpan = _tracer.ActiveSpan;
				var dictionary = new Dictionary<string, string>();

				if (activeSpan != null)
				{
					_tracer.Inject(activeSpan.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(dictionary));
				}

				await _busPublishService.PublishEvent(
					new BatchSubmissionCompletedEvent
					{
						BatchUri = batch.GetBatchUri,
						TracingKeys = dictionary
					},
					_rabbitMqTopologyConfiguration.ExchangeName,
					_rabbitMqTopologyConfiguration.RoutingKey);
			}

			await _batchRepository.SaveChangesAsync(cancellationToken);
		}
	}
}
