using System.IO;
using System.Text;
using System.Threading.Tasks;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using ClaimsSubmission.Infrastructure.Repositories;
using Infrastructure.Common.Web.Minio.Interfaces;
using Newtonsoft.Json;

namespace ClaimsSubmission.Infrastructure.Services
{
	public class BatchSubmissionFeedbackStoreService : IBatchSubmissionFeedbackStoreService
	{
		private readonly IObjectsStorageService _objectsStorageService;

		public BatchSubmissionFeedbackStoreService(IObjectsStorageService objectsStorageService)
		{
			_objectsStorageService = objectsStorageService;
		}

		public async Task<BatchFileContent> GetBatchFileContentAsync(string batchUri)
		{
			var objectResult = await _objectsStorageService.GetObjectContentAsync(
				BatchRepository.ObjectStorageBucket,
				batchUri);

			if (objectResult == null)
			{
				return null;
			}

			return JsonConvert.DeserializeObject<BatchFileContent>(objectResult.Content);
		}

		public async Task SaveFeedbackFile(Batch batch)
		{
			var feedbackContent = batch.GetBatchSubmissionFeedback;
			var feedbackUri = batch.GetFeedbackUri;

			var feedbackJson = JsonConvert.SerializeObject(feedbackContent);
			
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(feedbackJson)))
			{
				await _objectsStorageService.UploadObjectAsync(
					BatchRepository.ObjectStorageBucket,
					feedbackUri,
					stream);
			}
		}
	}
}