using System.Threading.Tasks;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public interface IBatchContentService
	{
		Task<BatchFileContent> GetBatchFileContentAsync(string batchUri);
	}
}