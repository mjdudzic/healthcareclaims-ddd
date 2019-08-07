using System;

namespace ClaimsSubmission.Domain.Exceptions
{
	public class ClaimSubmissionDomainException : Exception
	{
		public ClaimSubmissionDomainException()
		{ }

		public ClaimSubmissionDomainException(string message)
			: base(message)
		{ }

		public ClaimSubmissionDomainException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
