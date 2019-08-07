using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;

namespace ClaimsVetting.Infrastructure.Adapters
{
	public class DiagnosisTranslator
	{
		public Diagnosis ToDiagnosisFromRepresentation(dynamic treatmentCode)
		{
			return new Diagnosis
			{
				Code = treatmentCode.code,
				Description = treatmentCode.description
			};
		}
	}
}