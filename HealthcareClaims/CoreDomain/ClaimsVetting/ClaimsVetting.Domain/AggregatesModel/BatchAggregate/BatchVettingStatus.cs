using System;
using System.Collections.Generic;
using System.Linq;
using ClaimsVetting.Domain.Exceptions;
using ClaimsVetting.Domain.SeedWork;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public class BatchVettingStatus : Enumeration
	{
		public static BatchVettingStatus Started = new BatchVettingStatus(1, nameof(Started).ToLowerInvariant());
		public static BatchVettingStatus Accepted = new BatchVettingStatus(2, nameof(Accepted).ToLowerInvariant());
		public static BatchVettingStatus AcceptedWithWarnings = new BatchVettingStatus(3, nameof(AcceptedWithWarnings).ToLowerInvariant());
		public static BatchVettingStatus Rejected = new BatchVettingStatus(4, nameof(Rejected).ToLowerInvariant());

		public BatchVettingStatus(int id, string name) : base(id, name)
		{
		}

		public static IEnumerable<BatchVettingStatus> List() =>
			new[]
			{
				Started,
				Accepted,
				AcceptedWithWarnings,
				Rejected
			};

		public static BatchVettingStatus FromName(string name)
		{
			var state = List()
				.SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

			if (state == null)
			{
				throw new ClaimVettingDomainException(
					$"Possible values for BatchVettingStatus: {string.Join(",", List().Select(s => s.Name))}");
			}

			return state;
		}

		public static BatchVettingStatus From(int id)
		{
			var state = List().SingleOrDefault(s => s.Id == id);

			if (state == null)
			{
				throw new ClaimVettingDomainException(
					$"Possible values for BatchVettingStatus: {string.Join(",", List().Select(s => s.Name))}");
			}

			return state;
		}
	}
}
