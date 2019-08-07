using System.Collections.Generic;
using ClaimsVetting.Domain.SeedWork;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public class BatchVettingWarning : ValueObject
	{
		public string  FieldId { get; }

		public string WarningCode { get; }

		public string WarningDescription { get; }

		public BatchVettingWarning(string fieldId, string warningCode, string warningDescription)
		{
			FieldId = fieldId;
			WarningCode = warningCode;
			WarningDescription = warningDescription;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return FieldId;
			yield return WarningCode;
			yield return WarningDescription;
		}
	}
}