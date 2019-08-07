using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using ClaimsSubmission.Infrastructure.Persistence;
using Infrastructure.Common.Web.Minio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ClaimsSubmission.Infrastructure.Repositories
{
	public class BatchRepository : IBatchRepository
	{
		public const string ObjectStorageBucket = "healthcare-claims";

		private readonly BatchesContext _batchesContext;
		private readonly IObjectsStorageService _objectsStorageService;

		public BatchRepository(
			BatchesContext batchesContext,
			IObjectsStorageService objectsStorageService)
		{
			_batchesContext = batchesContext;
			_objectsStorageService = objectsStorageService;
		}

		public async Task<Batch> BatchForIdAsync(Guid id)
		{
			var batch = await _batchesContext
				.Batches
				.Include(i => i.BatchSubmissionStatus)
				.FirstOrDefaultAsync(i => i.Id == id);

			if (batch == null)
			{
				return null;
			}

			batch.BatchFileContent = await GetBatchFileContentAsync(batch.GetBatchUri);
			batch.BatchSubmissionFeedback = await GetFeedbackContentAsync(batch.GetFeedbackUri);

			return batch;
		}

		public Task AddBatchAsync(Batch batch)
		{
			_batchesContext.Batches.Add(batch);

			return _objectsStorageService.UploadObjectAsync(
				ObjectStorageBucket,
				batch.GetBatchUri,
				batch.GetBachFileContentStream);
		}

		public Task SaveBatchFeedbackAsync(string batchUri, Stream batchFileStream)
		{
			return _objectsStorageService.UploadObjectAsync(
				ObjectStorageBucket,
				batchUri,
				batchFileStream);
		}

		public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _batchesContext.SaveEntitiesAsync(cancellationToken);
		}

		private async Task<BatchFileContent> GetBatchFileContentAsync(string batchUri)
		{
			if (string.IsNullOrWhiteSpace(batchUri))
			{
				return null;
			}

			var objectResult = await _objectsStorageService.GetObjectContentAsync(
				ObjectStorageBucket,
				batchUri);

			if (objectResult == null)
			{
				return null;
			}

			return JsonConvert.DeserializeObject<BatchFileContent>(objectResult.Content);
		}

		private async Task<BatchSubmissionFeedback> GetFeedbackContentAsync(string feedbackUri)
		{
			if (string.IsNullOrWhiteSpace(feedbackUri))
			{
				return null;
			}

			var objectResult = await _objectsStorageService.GetObjectContentAsync(
				ObjectStorageBucket,
				feedbackUri);

			if (objectResult == null)
			{
				return null;
			}

			return JsonConvert.DeserializeObject<BatchSubmissionFeedback>(objectResult.Content);
		}
	}
}
