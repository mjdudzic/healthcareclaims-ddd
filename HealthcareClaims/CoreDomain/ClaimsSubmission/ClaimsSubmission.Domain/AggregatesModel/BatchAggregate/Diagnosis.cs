using System.Collections.Generic;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class Diagnosis : Treatment
	{
		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Code;
			yield return Description;
		}
	}
}
