using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Api.Application.BackgroundJobs;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using Hangfire;
using Infrastructure.Common.Web.RabbitMq;
using MediatR;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace ClaimsSubmission.Api.Application.Commands
{
	public class SubmitBatchCommandHandler : IRequestHandler<SubmitBatchCommand, Guid>
	{
		private readonly IBatchUriGenerator _batchUriGenerator;
		private readonly IBatchRepository _batchRepository;
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly ITracer _tracer;

		public SubmitBatchCommandHandler(
			IBatchUriGenerator batchUriGenerator,
			IBatchRepository batchRepository,
			IBackgroundJobClient backgroundJobClient,
			ITracer tracer)
		{
			_batchUriGenerator = batchUriGenerator;
			_batchRepository = batchRepository;
			_backgroundJobClient = backgroundJobClient;
			_tracer = tracer;
		}

		public async Task<Guid> Handle(SubmitBatchCommand request, CancellationToken cancellationToken)
		{
			using (var scope = _tracer.BuildSpan("batch-submit-command").StartActive(true))
			{
				var span = scope.Span.SetTag(Tags.SpanKind, Tags.SpanKindServer);

				var dictionary = new Dictionary<string, string>();
				_tracer.Inject(span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(dictionary));

				var batchId = Guid.NewGuid();
				var batchUri = _batchUriGenerator.GenerateBatchUri(request.HealthcareProviderId, batchId);

				using (var stream = request.BatchJsonFile.OpenReadStream())
				{
					var batch = new Batch(
						batchId,
						batchUri,
						stream);

					await _batchRepository.AddBatchAsync(batch);
				}

				await _batchRepository.SaveChangesAsync(cancellationToken);

				EnqueueFeedbackGenerationBackgroundJob(batchId, dictionary);

				return batchId;
			}
		}

		private void EnqueueFeedbackGenerationBackgroundJob(Guid batchId, Dictionary<string, string> tracingKeys)
		{
			var batchJobData = new BatchJobData
			{
				BatchId = batchId,
				TracingKeys = tracingKeys
			};

			_backgroundJobClient
				.Enqueue<GenerateBatchSubmissionFeedbackDocumentJob>(
					job => job.Execute(batchJobData, null, JobCancellationToken.Null));
		}
	}
}
