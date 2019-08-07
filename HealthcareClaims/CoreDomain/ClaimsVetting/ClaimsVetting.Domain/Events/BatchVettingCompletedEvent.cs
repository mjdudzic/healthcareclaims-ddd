using System;
using MediatR;

namespace ClaimsVetting.Domain.Events
{
	public class BatchVettingCompletedEvent : INotification
	{
		public Guid BatchId { get; }
		public string BatchUri { get; }
		public string VettingReportUri { get; }

		public BatchVettingCompletedEvent(
			Guid batchId,
			string batchUri,
			string vettingReportUri)
		{
			BatchId = batchId;
			BatchUri = batchUri;
			VettingReportUri = vettingReportUri;
		}
	}
}
