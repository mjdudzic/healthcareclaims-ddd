namespace ClaimsVetting.Infrastructure.Services
{
	public class BatchReportUriGenerator : IBatchReportUriGenerator
	{
		public string GenerateVettingReportUri(string batchUri)
		{
			return batchUri.Replace(".json", "-vetting-report.json");
		}
	}
}
