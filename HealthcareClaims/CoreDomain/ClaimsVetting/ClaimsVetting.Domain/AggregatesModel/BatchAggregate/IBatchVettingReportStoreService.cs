using System.Threading.Tasks;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public interface IBatchVettingReportStoreService
	{
		Task SaveReportFileAsync(Batch batch);
	}
}