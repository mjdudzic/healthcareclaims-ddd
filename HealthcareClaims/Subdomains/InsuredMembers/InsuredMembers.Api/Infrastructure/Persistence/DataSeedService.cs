using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InsuredMembers.Api.Infrastructure.Persistence
{
	public static class DataSeedService
	{
		public static void SeedData(IServiceProvider serviceProvider)
		{
			using (var context = new MembersContext(
				serviceProvider.GetRequiredService<DbContextOptions<MembersContext>>()))
			{
				if (context.Members.Any())
				{
					return;
				}

				context.Members.AddRange(
					CreateMember(1, "John", "Doe", "INS0000001", DateTime.Now.AddYears(-30), DateTime.Now.AddYears(1)),
					CreateMember(2, "Ann", "Doe", "INS0000002", DateTime.Now.AddYears(-40), DateTime.Now.AddYears(1)),
					CreateMember(3, "Michael", "Doe", "INS0000003", DateTime.Now.AddYears(-10), DateTime.Now.AddYears(1)));

				context.SaveChanges();
			}
		}

		private static Member CreateMember(
			int id,
			string firstName,
			string lastName,
			string insuranceNumber,
			DateTime birthDate,
			DateTime validTo)
		{
			return new Member
			{
				Id = id,
				FirstName = firstName,
				LastName = lastName,
				InsuranceNumber = insuranceNumber,
				BirthDate = birthDate,
				ValidTo = validTo
			};
		}
	}
}