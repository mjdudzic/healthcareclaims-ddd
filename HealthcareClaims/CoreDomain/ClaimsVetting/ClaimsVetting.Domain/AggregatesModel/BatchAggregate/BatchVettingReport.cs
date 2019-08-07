using System;
using System.Collections.Generic;
using ClaimsVetting.Domain.SeedWork;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public class BatchVettingReport : ValueObject
	{
		public Guid BatchId { get; set; }
		public List<BatchVettingError> BatchVettingErrors { get; set; }
		public List<BatchVettingWarning> BatchVettingWarnings { get; set; }

		public BatchVettingReport() { }

		public BatchVettingReport(
			Guid batchId,
			List<BatchVettingError> errors,
			List<BatchVettingWarning> warnings)
		{
			BatchId = batchId;
			BatchVettingErrors = errors;
			BatchVettingWarnings = warnings;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return BatchId;

			foreach (var error in BatchVettingErrors)
			{
				yield return error;
			}

			foreach (var warning in BatchVettingWarnings)
			{
				yield return warning;
			}
		}
	}
}