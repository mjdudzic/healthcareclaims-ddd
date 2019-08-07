using System.Collections.Generic;
using Infrastructure.Common.Web.RabbitMq;

namespace ClaimsSubmission.Api.Application.Messages.Events
{
	public class BusEventBase
	{
		public Dictionary<string, string> TracingKeys { get; set; }
	}
}