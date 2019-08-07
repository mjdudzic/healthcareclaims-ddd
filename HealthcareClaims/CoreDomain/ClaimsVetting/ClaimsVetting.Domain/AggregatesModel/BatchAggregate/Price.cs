using System.Collections.Generic;
using ClaimsVetting.Domain.SeedWork;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
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
