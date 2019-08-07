using MediatR;

namespace ClaimsSubmission.Api.Application.Messages.Events
{
	public class BatchSubmissionCompletedEvent : BusEventBase, INotification
	{
		public string BatchUri { get; set; }
	}
}