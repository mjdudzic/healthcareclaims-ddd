using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public interface IBatchRepository
	{
		Task<Batch> BatchForIdAsync(Guid id);
		Task AddBatchAsync(Batch batch);
		Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}