using System.Threading;
using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;
using ClaimsVetting.Infrastructure.Persistence;
using Infrastructure.Common.Web.Minio.Interfaces;

namespace ClaimsVetting.Infrastructure.Repositories
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

		public void AddBatch(Batch batch)
		{
			_batchesContext.Batches.Add(batch);
		}

		public Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _batchesContext.SaveEntitiesAsync(cancellationToken);
		}
	}
}
