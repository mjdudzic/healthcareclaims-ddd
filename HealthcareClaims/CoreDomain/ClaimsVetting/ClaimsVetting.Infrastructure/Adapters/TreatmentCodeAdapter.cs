using System.Net.Http;
using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;
using Newtonsoft.Json;

namespace ClaimsVetting.Infrastructure.Adapters
{
	public class TreatmentCodeAdapter
	{
		private readonly HttpClient _httpClient;

		public TreatmentCodeAdapter(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<Diagnosis> ToDiagnosisAsync(
			string apiEndpoint,
			string treatmentCode)
		{
			var responseContent =
				await GetResponseAsStringAsync($"{apiEndpoint}/treatments/{treatmentCode}");

			if (string.IsNullOrWhiteSpace(responseContent))
			{
				return null;
			}

			return new DiagnosisTranslator()
				.ToDiagnosisFromRepresentation(
					JsonConvert.DeserializeObject<dynamic>(responseContent));
		}

		public async Task<Procedure> ToProcedureAsync(
			string apiEndpoint,
			string treatmentCode)
		{
			var responseContent = 
				await GetResponseAsStringAsync($"{apiEndpoint}/treatments/{treatmentCode}");

			if (string.IsNullOrWhiteSpace(responseContent))
			{
				return null;
			}

			return new ProcedureTranslator()
				.ToProcedureFromRepresentation(
					JsonConvert.DeserializeObject<dynamic>(responseContent));
		}

		public async Task<Medicine> ToMedicineAsync(
			string apiEndpoint,
			string treatmentCode)
		{
			var responseContent =
				await GetResponseAsStringAsync($"{apiEndpoint}/medicines/{treatmentCode}");

			if (string.IsNullOrWhiteSpace(responseContent))
			{
				return null;
			}

			return new MedicineTranslator()
				.ToMedicineFromRepresentation(
					JsonConvert.DeserializeObject<dynamic>(responseContent));
		}

		private async Task<string> GetResponseAsStringAsync(string url)
		{
			var responseMessage = await _httpClient.GetAsync(url);

			if (responseMessage.IsSuccessStatusCode == false)
			{
				return null;
			}

			return await responseMessage.Content.ReadAsStringAsync();
		}
	}
}