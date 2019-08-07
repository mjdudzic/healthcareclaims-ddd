using System.Collections.Generic;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;

namespace ClaimsVetting.Domain.VettingEngine
{
	public class BatchVettingResult
	{
		public List<BatchVettingError> BatchVettingErrors { get; set; }
		public List<BatchVettingWarning> BatchVettingWarnings { get; set; }

		public BatchVettingResult()
		{
			BatchVettingErrors = new List<BatchVettingError>();
			BatchVettingWarnings = new List<BatchVettingWarning>();
		}
	}
}