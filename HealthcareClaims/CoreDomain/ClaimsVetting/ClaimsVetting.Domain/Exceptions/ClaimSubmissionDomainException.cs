using System;

namespace ClaimsVetting.Domain.Exceptions
{
	public class ClaimVettingDomainException : Exception
	{
		public ClaimVettingDomainException()
		{ }

		public ClaimVettingDomainException(string message)
			: base(message)
		{ }

		public ClaimVettingDomainException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
