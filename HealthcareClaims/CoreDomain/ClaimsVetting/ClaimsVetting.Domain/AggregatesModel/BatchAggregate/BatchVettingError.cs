using System.Collections.Generic;
using ClaimsVetting.Domain.SeedWork;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public class BatchVettingError : ValueObject
	{
		public string  FieldId { get; }

		public string ErrorCode { get; }

		public string ErrorDescription { get; }

		public BatchVettingError(string fieldId, string errorCode, string errorDescription)
		{
			FieldId = fieldId;
			ErrorCode = errorCode;
			ErrorDescription = errorDescription;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return FieldId;
			yield return ErrorCode;
			yield return ErrorDescription;
		}
	}
}