using System.Linq;
using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;

namespace ClaimsVetting.Domain.VettingEngine
{
	public class ProcedureVettingPlugin : IBatchVettingPlugin
	{
		private readonly IDictionaryCodesService _dictionaryCodesService;

		public ProcedureVettingPlugin(IDictionaryCodesService dictionaryCodesService)
		{
			_dictionaryCodesService = dictionaryCodesService;
		}

		public async Task<BatchVettingResult> VetAsync(Batch batch)
		{
			var batchContent = batch.GetBatchFileContent;
			var proceduresCodes = batchContent
				.Claims
				.SelectMany(i => i.Procedures.Select(d => d.Code))
				.ToList();

			var codes = await _dictionaryCodesService.GetProceduresAsync(proceduresCodes);

			var result = new BatchVettingResult();

			foreach (var claim in batchContent.Claims)
			{
				var codesNotFound = claim.Procedures
					.Where(d => codes.Any(c => c.Code == d.Code) == false)
					.Select(i => i.Code)
					.ToList();

				if (codesNotFound.Any() == false)
				{
					continue;
				}

				result.BatchVettingErrors.Add(new BatchVettingError(
					claim.ClaimNumber, 
					"ERR_PROCEDURE_NOT_FOUND",
					$"Procedures not found: {string.Join(",", codesNotFound)}"));
			}

			return result;
		}
	}
}