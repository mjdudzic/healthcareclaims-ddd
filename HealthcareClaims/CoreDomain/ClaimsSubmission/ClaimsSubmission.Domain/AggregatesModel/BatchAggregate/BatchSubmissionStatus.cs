using System;
using System.Collections.Generic;
using System.Linq;
using ClaimsSubmission.Domain.Exceptions;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class BatchSubmissionStatus : Enumeration
	{
		public static BatchSubmissionStatus Started = new BatchSubmissionStatus(1, nameof(Started).ToLowerInvariant());
		public static BatchSubmissionStatus InProgress = new BatchSubmissionStatus(2, nameof(InProgress).ToLowerInvariant());
		public static BatchSubmissionStatus Accepted = new BatchSubmissionStatus(3, nameof(Accepted).ToLowerInvariant());
		public static BatchSubmissionStatus Rejected = new BatchSubmissionStatus(4, nameof(Rejected).ToLowerInvariant());

		public BatchSubmissionStatus(int id, string name) : base(id, name)
		{
		}

		public static IEnumerable<BatchSubmissionStatus> List() =>
			new[]
			{
				Started,
				InProgress,
				Accepted,
				Rejected
			};

		public static BatchSubmissionStatus FromName(string name)
		{
			var state = List()
				.SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

			if (state == null)
			{
				throw new ClaimSubmissionDomainException(
					$"Possible values for BatchSubmissionStatus: {string.Join(",", List().Select(s => s.Name))}");
			}

			return state;
		}

		public static BatchSubmissionStatus From(int id)
		{
			var state = List().SingleOrDefault(s => s.Id == id);

			if (state == null)
			{
				throw new ClaimSubmissionDomainException(
					$"Possible values for BatchSubmissionStatus: {string.Join(",", List().Select(s => s.Name))}");
			}

			return state;
		}
	}
}
