using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodesDictionary.Api.Infrastructure.Persistence
{
	public static class DataSeedService
	{
		public static void SeedData(IServiceProvider serviceProvider)
		{
			using (var context = new CodesContext(
				serviceProvider.GetRequiredService<DbContextOptions<CodesContext>>()))
			{
				if (context.TreatmentCodes.Any() == false)
				{
					AddTreatmentCodes(context);
				}

				if (context.MedicineCodes.Any() == false)
				{
					AddMedicineCodes(context);
				}

				context.SaveChanges();
			}
		}

		private static void AddTreatmentCodes(CodesContext context)
		{
			context.TreatmentCodes.Add(new TreatmentCode
			{
				Id = 1,
				Code = "D01",
				Description = "Some diagnose",
				CodeType = (int) CodeType.Diagnosis
			});
			context.TreatmentCodes.Add(new TreatmentCode
			{
				Id = 2,
				Code = "D02",
				Description = "Some diagnose",
				CodeType = (int)CodeType.Diagnosis
			});
			context.TreatmentCodes.Add(new TreatmentCode
			{
				Id = 3,
				Code = "D03",
				Description = "Some diagnose",
				CodeType = (int)CodeType.Diagnosis
			});

			context.TreatmentCodes.Add(new TreatmentCode
			{
				Id = 4,
				Code = "P01",
				Description = "Some procedure",
				CodeType = (int)CodeType.Procedure
			});
			context.TreatmentCodes.Add(new TreatmentCode
			{
				Id = 5,
				Code = "P02",
				Description = "Some procedure",
				CodeType = (int)CodeType.Procedure
			});
			context.TreatmentCodes.Add(new TreatmentCode
			{
				Id = 6,
				Code = "P03",
				Description = "Some procedure",
				CodeType = (int)CodeType.Procedure
			});
		}

		private static void AddMedicineCodes(CodesContext context)
		{
			context.MedicineCodes.Add(new MedicineCode
			{
				Id = 1,
				Code = "M01",
				Description = "Some medicine",
				Price = 100,
				UnitName = "tablets"
			});
			context.MedicineCodes.Add(new MedicineCode
			{
				Id = 2,
				Code = "M02",
				Description = "Some medicine",
				Price = 100,
				UnitName = "tablets"
			});
			context.MedicineCodes.Add(new MedicineCode
			{
				Id = 3,
				Code = "M03",
				Description = "Some medicine",
				Price = 100,
				UnitName = "tablets"
			});
		}
	}
}