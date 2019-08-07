using System.Linq;
using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;

namespace ClaimsVetting.Domain.VettingEngine
{
	public class DiagnosisVettingPlugin : IBatchVettingPlugin
	{
		private readonly IDictionaryCodesService _dictionaryCodesService;

		public DiagnosisVettingPlugin(IDictionaryCodesService dictionaryCodesService)
		{
			_dictionaryCodesService = dictionaryCodesService;
		}

		public async Task<BatchVettingResult> VetAsync(Batch batch)
		{
			var batchContent = batch.GetBatchFileContent;
			var diagnosisCodes = batchContent
				.Claims
				.SelectMany(i => i.Diagnoses.Select(d => d.Code))
				.ToList();

			var codes = await _dictionaryCodesService.GetDiagnosesAsync(diagnosisCodes);

			var result = new BatchVettingResult();

			foreach (var claim in batchContent.Claims)
			{
				var codesNotFound = claim.Diagnoses
					.Where(d => codes.Any(c => c.Code == d.Code) == false)
					.Select(i => i.Code)
					.ToList();

				if (codesNotFound.Any() == false)
				{
					continue;
				}

				result.BatchVettingErrors.Add(new BatchVettingError(
					claim.ClaimNumber, 
					"ERR_DIAGNOSIS_NOT_FOUND",
					$"Diagnoses not found: {string.Join(",", codesNotFound)}"));
			}

			return result;
		}
	}
}