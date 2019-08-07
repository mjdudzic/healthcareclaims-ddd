using System.Collections.Generic;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class MedicineUnit : ValueObject
	{
		public decimal Quantity { get; set; }
		public string UnitName { get; set; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Quantity;
			yield return UnitName;
		}
	}
}
