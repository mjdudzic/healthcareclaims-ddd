using System.Collections.Generic;
using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;
using ClaimsVetting.Domain.VettingEngine;
using ClaimsVetting.Infrastructure.Services;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Infrastructure.Common.Web.Jaeger;
using OpenTracing;

namespace ClaimsVetting.Api.Application.BackgroundJobs
{
	public class VetBatchJob : IJob<BatchJobData>
	{
		private readonly IBatchRepository _batchRepository;
		private readonly IBatchContentService _batchContentService;
		private readonly IBatchReportUriGenerator _batchReportUriGenerator;
		private readonly IEnumerable<IBatchVettingPlugin> _vettingPlugins;
		private readonly IBatchVettingReportStoreService _batchVettingReportStoreService;
		private readonly ITracer _tracer;

		public VetBatchJob(
			IBatchRepository batchRepository,
			IBatchContentService batchContentService,
			IBatchReportUriGenerator batchReportUriGenerator,
			IEnumerable<IBatchVettingPlugin> vettingPlugins,
			IBatchVettingReportStoreService batchVettingReportStoreService,
			ITracer tracer)
		{
			_batchRepository = batchRepository;
			_batchContentService = batchContentService;
			_batchReportUriGenerator = batchReportUriGenerator;
			_vettingPlugins = vettingPlugins;
			_batchVettingReportStoreService = batchVettingReportStoreService;
			_tracer = tracer;
		}

		public async Task Execute(BatchJobData batchJobData, PerformContext context, IJobCancellationToken cancellationToken)
		{
			using (TracingExtensions.StartServerSpan(_tracer, batchJobData.TracingKeys, "batch-vetting-job"))
			{
				context.WriteLine($"Generating feedback doc for batch {batchJobData.BatchUri}");

				var batch = await Batch.CreateAsync(
					batchJobData.BatchUri,
					_batchContentService);

				_batchRepository.AddBatch(batch);

				await _batchRepository.SaveChangesAsync(cancellationToken.ShutdownToken);

				await batch.Vet(
					_batchReportUriGenerator.GenerateVettingReportUri(batch.GetBatchUri),
					_vettingPlugins,
					_batchVettingReportStoreService);

				await _batchRepository.SaveChangesAsync(cancellationToken.ShutdownToken);
			}
		}
	}
}