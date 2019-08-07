using System;

namespace InsuredMembers.Api.Infrastructure.Persistence
{
	public class Member
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDate { get; set; }
		public string InsuranceNumber { get; set; }
		public DateTime ValidTo { get; set; }
	}
}