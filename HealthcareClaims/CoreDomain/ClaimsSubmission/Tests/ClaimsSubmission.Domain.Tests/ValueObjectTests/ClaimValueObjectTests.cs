using System;
using System.Collections.Generic;
using System.Linq;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using FluentAssertions;
using Xunit;

namespace Domain.Tests.ValueObjectTests
{
	public class ClaimValueObjectTests
	{
		[Fact]
		public void GivenTwoClaimsWithTheSameValues_WhenCompared_ThenReturnsTrue()
		{
			// Arrange
			var claim1 = GetTestClaimInstance();
			var claim2 = GetTestClaimWithCopiedValuesFrom(claim1);

			// Act
			var result = claim1.Equals(claim2);

			// Assert
			result.Should().BeTrue();
		}

		private Claim GetTestClaimInstance()
		{
			return new Claim
			{
				ClaimNumber = Guid.NewGuid().ToString(),
				SubmissionDate = DateTime.UtcNow,
				TotalPrice = new Price
				{
					Amount = new Random().Next(100000),
					Currency = "PLN"
				},
				Patient = new Patient
				{
					InsuranceNumber = Guid.NewGuid().ToString(),
					BirthDate = DateTime.UtcNow,
					FirstName = Guid.NewGuid().ToString(),
					LastName = Guid.NewGuid().ToString(),
					Gender = GenderType.Undefined,
					VisitDate = DateTime.Now
				},
				Diagnoses = new List<Diagnosis>
				{
					new Diagnosis
					{
						Code = Guid.NewGuid().ToString(),
						Description = Guid.NewGuid().ToString()
					}
				},
				Procedures = new List<Procedure>
				{
					new Procedure
					{
						Code = Guid.NewGuid().ToString(),
						Description = Guid.NewGuid().ToString()
					}
				},
				Medicines = new List<Medicine>
				{
					new Medicine
					{
						Code = Guid.NewGuid().ToString(),
						Description = Guid.NewGuid().ToString(),
						PriceForUnit = new Price
						{
							Amount = new Random().Next(100000),
							Currency = "PLN"
						},
						Unit = new MedicineUnit
						{
							Quantity = new Random().Next(100),
							UnitName = Guid.NewGuid().ToString()
						}
					}
				}
			};
		}

		private Claim GetTestClaimWithCopiedValuesFrom(Claim claim)
		{
			return new Claim
			{
				ClaimNumber = claim.ClaimNumber,
				SubmissionDate = claim.SubmissionDate,
				TotalPrice = new Price
				{
					Amount = claim.TotalPrice.Amount,
					Currency = claim.TotalPrice.Currency
				},
				Patient = new Patient
				{
					InsuranceNumber = claim.Patient.InsuranceNumber,
					BirthDate = claim.Patient.BirthDate,
					FirstName = claim.Patient.FirstName,
					LastName = claim.Patient.LastName,
					Gender = claim.Patient.Gender,
					VisitDate = claim.Patient.VisitDate
				},
				Diagnoses = new List<Diagnosis>
				{
					new Diagnosis
					{
						Code = claim.Diagnoses.First().Code,
						Description = claim.Diagnoses.First().Description
					}
				},
				Procedures = new List<Procedure>
				{
					new Procedure
					{
						Code = claim.Procedures.First().Code,
						Description = claim.Procedures.First().Description
					}
				},
				Medicines = new List<Medicine>
				{
					new Medicine
					{
						Code = claim.Medicines.First().Code,
						Description = claim.Medicines.First().Description,
						PriceForUnit = new Price
						{
							Amount = claim.Medicines.First().PriceForUnit.Amount,
							Currency = claim.Medicines.First().PriceForUnit.Currency
						},
						Unit = new MedicineUnit
						{
							Quantity = claim.Medicines.First().Unit.Quantity,
							UnitName = claim.Medicines.First().Unit.UnitName
						}
					}
				}
			};
		}
	}
}
