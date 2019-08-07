using System.Collections.Generic;

namespace ClaimsVetting.Api.Application.IntegrationEvents.Events
{
	public class BusEventBase
	{
		public Dictionary<string, string> TracingKeys { get; set; }
	}
}