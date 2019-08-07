using System.Collections.Generic;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class Medicine : Treatment
	{
		public MedicineUnit Unit { get; set; }
		public Price PriceForUnit { get; set; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Code;
			yield return Description;
			yield return Unit;
			yield return PriceForUnit;
		}
	}
}
