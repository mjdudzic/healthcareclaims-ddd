using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;
using ClaimsVetting.Infrastructure.Repositories;
using Infrastructure.Common.Web.Minio.Interfaces;
using Newtonsoft.Json;

namespace ClaimsVetting.Infrastructure.Services
{
	public class BatchContentService : IBatchContentService
	{
		private readonly IObjectsStorageService _objectsStorageService;

		public BatchContentService(IObjectsStorageService objectsStorageService)
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
	}
}