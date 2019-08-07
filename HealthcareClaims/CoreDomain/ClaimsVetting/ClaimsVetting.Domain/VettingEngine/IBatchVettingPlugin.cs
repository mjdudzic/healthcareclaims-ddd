using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;

namespace ClaimsVetting.Domain.VettingEngine
{
	public interface IBatchVettingPlugin
	{
		Task<BatchVettingResult> VetAsync(Batch batch);
	}
}