using System.Collections.Generic;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class Price : ValueObject
	{
		public decimal Amount { get; set; }
		public string Currency { get; set; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Amount;
			yield return Currency;
		}
	}
}
