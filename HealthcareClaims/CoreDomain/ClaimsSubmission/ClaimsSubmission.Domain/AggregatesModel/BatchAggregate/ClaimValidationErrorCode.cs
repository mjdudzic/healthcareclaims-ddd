using System;
using System.Collections.Generic;
using System.Linq;
using ClaimsSubmission.Domain.Exceptions;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class ClaimValidationErrorCode : Enumeration
	{
		public static ClaimValidationErrorCode ClaimIsDuplicate = new ClaimValidationErrorCode(1, nameof(ClaimIsDuplicate).ToLowerInvariant());
		public static ClaimValidationErrorCode ClaimNumberInvalid = new ClaimValidationErrorCode(1, nameof(ClaimNumberInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode SubmissionDateInvalid = new ClaimValidationErrorCode(2, nameof(SubmissionDateInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode TotalPriceInvalid = new ClaimValidationErrorCode(3, nameof(TotalPriceInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode PatientInsuranceNumberInvalid = new ClaimValidationErrorCode(4, nameof(PatientInsuranceNumberInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode PatientBirthDateInvalid = new ClaimValidationErrorCode(5, nameof(PatientBirthDateInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode PatientGenderInvalid = new ClaimValidationErrorCode(6, nameof(PatientGenderInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode PatientVisitDateInvalid = new ClaimValidationErrorCode(7, nameof(PatientVisitDateInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode DiagnosisCodeInvalid = new ClaimValidationErrorCode(8, nameof(DiagnosisCodeInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode ProcedureCodeInvalid = new ClaimValidationErrorCode(9, nameof(ProcedureCodeInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode MedicineCodeInvalid = new ClaimValidationErrorCode(10, nameof(MedicineCodeInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode MedicineUnitInvalid = new ClaimValidationErrorCode(11, nameof(MedicineUnitInvalid).ToLowerInvariant());
		public static ClaimValidationErrorCode MedicinePriceInvalid = new ClaimValidationErrorCode(12, nameof(MedicinePriceInvalid).ToLowerInvariant());

		public ClaimValidationErrorCode(int id, string name) : base(id, name)
		{
		}

		public static IEnumerable<ClaimValidationErrorCode> List() =>
			new[]
			{
				ClaimIsDuplicate,
				ClaimNumberInvalid,
				SubmissionDateInvalid,
				TotalPriceInvalid,
				PatientInsuranceNumberInvalid,
				PatientBirthDateInvalid,
				PatientGenderInvalid,
				PatientVisitDateInvalid,
				DiagnosisCodeInvalid,
				ProcedureCodeInvalid,
				MedicineCodeInvalid,
				MedicineUnitInvalid,
				MedicinePriceInvalid
			};

		public static ClaimValidationErrorCode FromName(string name)
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

		public static ClaimValidationErrorCode From(int id)
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
