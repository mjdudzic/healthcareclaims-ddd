using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;

namespace ClaimsVetting.Infrastructure.Adapters
{
	public class MedicineTranslator
	{
		public Medicine ToMedicineFromRepresentation(dynamic treatmentCode)
		{
			return new Medicine
			{
				Code = treatmentCode.code,
				Description = treatmentCode.description,
				PriceForUnit = new Price
				{
					Amount = treatmentCode.price
				},
				Unit = new MedicineUnit
				{
					UnitName = treatmentCode.unitName
				}
			};
		}
	}
}