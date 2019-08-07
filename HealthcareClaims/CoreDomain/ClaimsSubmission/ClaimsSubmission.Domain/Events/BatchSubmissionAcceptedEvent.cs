using System;
using MediatR;

namespace ClaimsSubmission.Domain.Events
{
	public class BatchSubmissionAcceptedEvent : INotification
	{
		public Guid BatchId { get; }

		public BatchSubmissionAcceptedEvent(Guid batchId)
		{
			BatchId = batchId;
		}
	}
}
