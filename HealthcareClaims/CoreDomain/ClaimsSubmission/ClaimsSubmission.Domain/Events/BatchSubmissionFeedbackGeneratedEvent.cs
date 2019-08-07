using System;
using MediatR;

namespace ClaimsSubmission.Domain.Events
{
	public class BatchSubmissionFeedbackGeneratedEvent : INotification
	{
		public Guid BatchId { get; }
		public string BatchFeedbackUri { get; }
		public bool HasErrors { get; }

		public BatchSubmissionFeedbackGeneratedEvent(
			Guid batchId,
			string feedbackUri,
			bool hasErrors)
		{
			BatchId = batchId;
			BatchFeedbackUri = feedbackUri;
			HasErrors = hasErrors;
		}
	}
}
