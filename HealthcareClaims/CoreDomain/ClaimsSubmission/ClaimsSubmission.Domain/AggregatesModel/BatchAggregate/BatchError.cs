using System.Collections.Generic;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class BatchError : ValueObject
	{
		public FeedbackFieldLevelType FieldLevelType { get; }

		public string  FieldId { get; }

		public string ErrorCode { get; }

		public string ErrorDescription { get; }

		public BatchError(FeedbackFieldLevelType fieldLevelType, string fieldId, string errorCode, string errorDescription)
		{
			FieldLevelType = fieldLevelType;
			FieldId = fieldId;
			ErrorCode = errorCode;
			ErrorDescription = errorDescription;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return FieldLevelType;
			yield return FieldId;
			yield return ErrorCode;
			yield return ErrorDescription;
		}
	}
}