using MediatR;

namespace ClaimsVetting.Api.Application.IntegrationEvents.Events
{
	public class BatchSubmissionCompletedEvent : BusEventBase, INotification
	{
		public string BatchUri { get; set; }
	}
}