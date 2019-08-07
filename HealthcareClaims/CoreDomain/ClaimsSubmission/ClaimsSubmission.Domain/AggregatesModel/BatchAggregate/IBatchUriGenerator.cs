using System;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public interface IBatchUriGenerator
	{
		string GenerateBatchUri(Guid healthcareProviderId, Guid batchId);
		string GenerateSubmissionFeedbackUri(string batchUri);
	}
}