using System.Threading;
using System.Threading.Tasks;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public interface IBatchRepository
	{
		void AddBatch(Batch batch);
		Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}