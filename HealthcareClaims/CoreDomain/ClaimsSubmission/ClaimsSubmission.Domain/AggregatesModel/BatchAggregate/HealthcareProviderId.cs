using System;
using System.Collections.Generic;
using System.Text;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class HealthcareProviderId : ValueObject
	{
		public string IdentificationNumber { get; set; }
		public string RegionCode { get; set; }

		private HealthcareProviderId() { }

		public HealthcareProviderId(string identificationNumber, string regionCode)
		{
			IdentificationNumber = identificationNumber;
			RegionCode = regionCode;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return IdentificationNumber;
			yield return RegionCode;
		}
	}
}
