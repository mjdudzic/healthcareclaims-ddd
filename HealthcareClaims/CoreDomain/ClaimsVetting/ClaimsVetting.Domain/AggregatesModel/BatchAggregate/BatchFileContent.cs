using System;
using System.Collections.Generic;
using ClaimsVetting.Domain.SeedWork;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public class BatchFileContent : ValueObject
	{
		public string BatchNumber { get; set; }
		public DateTime SubmissionDate { get; set; }
		public HealthcareProviderId HealthcareProviderId { get; set; }
		public List<Claim> Claims { get; set; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return BatchNumber;
			yield return SubmissionDate;
			yield return HealthcareProviderId;

			foreach (var claim in Claims)
			{
				yield return claim;
			}
		}
	}
}
