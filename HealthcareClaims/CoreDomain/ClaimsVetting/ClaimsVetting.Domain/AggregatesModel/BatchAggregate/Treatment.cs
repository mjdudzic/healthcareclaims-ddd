using ClaimsVetting.Domain.SeedWork;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public abstract class Treatment : ValueObject
	{
		public string Code { get; set; }
		public string Description { get; set; }
	}
}
