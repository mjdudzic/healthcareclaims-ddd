using System;
using MediatR;

namespace ClaimsSubmission.Domain.Events
{
	public class BatchSubmissionRejectedEvent : INotification
	{
		public Guid BatchId { get; }

		public BatchSubmissionRejectedEvent(Guid batchId)
		{
			BatchId = batchId;
		}
	}
}
