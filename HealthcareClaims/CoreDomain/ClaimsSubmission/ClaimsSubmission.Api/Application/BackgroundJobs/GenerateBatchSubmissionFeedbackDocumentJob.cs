using System;
using System.Threading.Tasks;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Infrastructure.Common.Web.Jaeger;
using OpenTracing;

namespace ClaimsSubmission.Api.Application.BackgroundJobs
{
	public class GenerateBatchSubmissionFeedbackDocumentJob : IJob<BatchJobData>
	{
		private readonly IBatchRepository _batchRepository;
		private readonly IBatchUriGenerator _batchUriGenerator;
		private readonly IBatchSubmissionFeedbackStoreService _batchSubmissionFeedbackStoreService;
		private readonly ITracer _tracer;

		public GenerateBatchSubmissionFeedbackDocumentJob(
			IBatchRepository batchRepository,
			IBatchUriGenerator batchUriGenerator,
			IBatchSubmissionFeedbackStoreService batchSubmissionFeedbackStoreService,
			ITracer tracer)
		{
			_batchRepository = batchRepository;
			_batchUriGenerator = batchUriGenerator;
			_batchSubmissionFeedbackStoreService = batchSubmissionFeedbackStoreService;
			_tracer = tracer;
		}

		public async Task Execute(BatchJobData batchJobData, PerformContext context, IJobCancellationToken cancellationToken)
		{
			using (TracingExtensions.StartServerSpan(_tracer, batchJobData.TracingKeys, "batch-submission-feedback-generation"))
			{
				context.WriteLine($"Generating feedback doc for batch {batchJobData.BatchId}");

				var batch = await _batchRepository.BatchForIdAsync(batchJobData.BatchId);

				var feedbackUri = _batchUriGenerator.GenerateSubmissionFeedbackUri(batch.GetBatchUri);

				await batch.GenerateSubmissionFeedbackAsync(
					feedbackUri,
					_batchSubmissionFeedbackStoreService);

				await _batchRepository.SaveChangesAsync(cancellationToken.ShutdownToken);
			}
		}
	}
}