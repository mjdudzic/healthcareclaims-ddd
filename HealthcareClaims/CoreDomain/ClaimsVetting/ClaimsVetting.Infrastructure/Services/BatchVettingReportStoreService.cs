using System.IO;
using System.Text;
using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;
using ClaimsVetting.Infrastructure.Repositories;
using Infrastructure.Common.Web.Minio.Interfaces;
using Newtonsoft.Json;

namespace ClaimsVetting.Infrastructure.Services
{
	public class BatchVettingReportStoreService : IBatchVettingReportStoreService
	{
		private readonly IObjectsStorageService _objectsStorageService;

		public BatchVettingReportStoreService(IObjectsStorageService objectsStorageService)
		{
			_objectsStorageService = objectsStorageService;
		}

		public async Task SaveReportFileAsync(Batch batch)
		{
			var report = batch.GetBatchVettingReport;
			var reportUri = batch.GetVettingReportUri;

			var feedbackJson = JsonConvert.SerializeObject(report);

			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(feedbackJson)))
			{
				await _objectsStorageService.UploadObjectAsync(
					BatchRepository.ObjectStorageBucket,
					reportUri,
					stream);
			}
		}
	}
}