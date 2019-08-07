using System.Collections.Generic;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class Procedure : Treatment
	{
		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Code;
			yield return Description;
		}
	}
}
