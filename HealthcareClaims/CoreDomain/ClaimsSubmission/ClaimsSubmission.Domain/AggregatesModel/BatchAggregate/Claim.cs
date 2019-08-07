using System;
using System.Collections.Generic;
using System.Linq;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class Claim : ValueObject
	{
		public string ClaimNumber { get; set; }
		public DateTime SubmissionDate { get; set; }
		public Price TotalPrice { get; set; }
		public Patient Patient { get; set; }

		public List<Diagnosis> Diagnoses { get; set; }

		public List<Procedure> Procedures { get; set; }

		public List<Medicine> Medicines { get; set; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return ClaimNumber;
			yield return SubmissionDate;
			yield return TotalPrice;
			yield return Patient;

			foreach (var diagnosis in Diagnoses)
			{
				yield return diagnosis;
			}

			foreach (var procedure in Procedures)
			{
				yield return procedure;
			}

			foreach (var medicine in Medicines)
			{
				yield return medicine;
			}
		}
	}
}
