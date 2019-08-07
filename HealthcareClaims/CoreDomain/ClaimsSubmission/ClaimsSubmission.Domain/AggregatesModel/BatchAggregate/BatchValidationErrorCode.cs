using System;
using System.Collections.Generic;
using System.Linq;
using ClaimsSubmission.Domain.Exceptions;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class BatchValidationErrorCode : Enumeration
	{
		public static BatchValidationErrorCode HealthcareProviderIdInvalid = new BatchValidationErrorCode(1, nameof(HealthcareProviderIdInvalid).ToLowerInvariant());

		public BatchValidationErrorCode(int id, string name) : base(id, name)
		{
		}

		public static IEnumerable<BatchValidationErrorCode> List() =>
			new[]
			{
				HealthcareProviderIdInvalid
			};

		public static BatchValidationErrorCode FromName(string name)
		{
			var state = List()
				.SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

			if (state == null)
			{
				throw new ClaimSubmissionDomainException(
					$"Possible values for BatchValidationErrorCode: {string.Join(",", List().Select(s => s.Name))}");
			}

			return state;
		}

		public static BatchValidationErrorCode From(int id)
		{
			var state = List().SingleOrDefault(s => s.Id == id);

			if (state == null)
			{
				throw new ClaimSubmissionDomainException(
					$"Possible values for BatchValidationErrorCode: {string.Join(",", List().Select(s => s.Name))}");
			}

			return state;
		}
	}
}
