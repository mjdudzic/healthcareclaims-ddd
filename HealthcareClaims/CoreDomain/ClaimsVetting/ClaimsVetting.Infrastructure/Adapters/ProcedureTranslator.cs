using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;

namespace ClaimsVetting.Infrastructure.Adapters
{
	public class ProcedureTranslator
	{
		public Procedure ToProcedureFromRepresentation(dynamic treatmentCode)
		{
			return new Procedure
			{
				Code = treatmentCode.code,
				Description = treatmentCode.description
			};
		}
	}
}