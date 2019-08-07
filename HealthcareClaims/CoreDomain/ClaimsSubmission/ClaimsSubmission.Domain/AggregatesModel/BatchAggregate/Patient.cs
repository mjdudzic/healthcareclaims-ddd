using System;
using System.Collections.Generic;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class Patient : ValueObject
	{
		public string InsuranceNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDate { get; set; }
		public GenderType Gender { get; set; }
		public DateTime VisitDate { get; set; }
		public DateTime? DischargeDate { get; set; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return InsuranceNumber;
			yield return FirstName;
			yield return LastName;
			yield return BirthDate;
			yield return Gender;
			yield return VisitDate;
			yield return DischargeDate;
		}
	}
}
