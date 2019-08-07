using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ClaimsSubmission.Domain.Events
{
	public class BatchSubmissionStartedEvent : INotification
	{
		public Guid BatchId { get; }
		public string BatchUri { get; }
		public DateTime CreationDate { get; }

		public BatchSubmissionStartedEvent(
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
