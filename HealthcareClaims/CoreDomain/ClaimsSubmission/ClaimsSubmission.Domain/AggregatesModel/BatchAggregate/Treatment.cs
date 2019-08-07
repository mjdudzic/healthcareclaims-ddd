using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public abstract class Treatment : ValueObject
	{
		public string Code { get; set; }
		public string Description { get; set; }
	}
}
