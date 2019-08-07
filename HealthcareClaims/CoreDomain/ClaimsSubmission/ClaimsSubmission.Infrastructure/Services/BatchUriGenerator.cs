using System;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;

namespace ClaimsSubmission.Infrastructure.Services
{
	public class BatchUriGenerator : IBatchUriGenerator
	{
		public string GenerateBatchUri(Guid healthcareProviderId, Guid batchId)
		{
			return $"HealthcareProviders/{healthcareProviderId}/batches/{batchId}.json";
		}

		public string GenerateSubmissionFeedbackUri(string batchUri)
		{
			return batchUri.Replace(".json", "-feedback.json");
		}
	}
}
