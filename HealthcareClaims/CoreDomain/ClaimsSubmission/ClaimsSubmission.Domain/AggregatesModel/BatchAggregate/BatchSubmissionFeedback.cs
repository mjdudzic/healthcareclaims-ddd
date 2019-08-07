using System;
using System.Collections.Generic;
using System.Linq;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class BatchSubmissionFeedback : ValueObject
	{
		public Guid BatchId { get; set; }
		public bool HasErrors { get; set; }
		public List<BatchError> BatchErrors { get;  set; }

		public BatchSubmissionFeedback() { }

		public BatchSubmissionFeedback(
			Guid batchId,
			List<BatchError> errors)
		{
			BatchId = batchId;
			BatchErrors = errors;

			if (errors != null && errors.Any())
			{
				HasErrors = true;
			}
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return BatchId;
			yield return HasErrors;

			foreach (var batchError in BatchErrors)
			{
				yield return batchError;
			}
		}
	}
}