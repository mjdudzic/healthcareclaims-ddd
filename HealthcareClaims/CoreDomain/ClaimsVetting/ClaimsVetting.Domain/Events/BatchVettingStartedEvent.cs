using System;
using MediatR;

namespace ClaimsVetting.Domain.Events
{
	public class BatchVettingStartedEvent : INotification
	{
		public Guid BatchId { get; }
		public string BatchUri { get; }
		public DateTime CreationDate { get; }

		public BatchVettingStartedEvent(
			Guid batchId,
			string batchUri,
			DateTime creationDate)
		{
			BatchId = batchId;
			BatchUri = batchUri;
			CreationDate = creationDate;
		}
	}
}
