using System.Threading.Tasks;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public interface IBatchSubmissionFeedbackStoreService
	{
		Task SaveFeedbackFile(Batch batch);
	}
}