using System.Collections.Generic;

namespace ClaimsVetting.Api.Application.BackgroundJobs
{
	public class BatchJobData
	{
		public string BatchUri { get; set; }
		public Dictionary<string, string> TracingKeys { get; set; }
	}
}