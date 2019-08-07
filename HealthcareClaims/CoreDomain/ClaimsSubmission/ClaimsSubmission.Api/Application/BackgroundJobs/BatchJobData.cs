using System;
using System.Collections.Generic;
using Infrastructure.Common.Web.RabbitMq;

namespace ClaimsSubmission.Api.Application.BackgroundJobs
{
	public class BatchJobData
	{
		public Guid BatchId { get; set; }
		public Dictionary<string, string> TracingKeys { get; set; }
	}
}