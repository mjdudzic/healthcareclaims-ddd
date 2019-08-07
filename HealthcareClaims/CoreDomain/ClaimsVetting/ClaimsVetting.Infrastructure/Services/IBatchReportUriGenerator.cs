namespace ClaimsVetting.Infrastructure.Services
{
	public interface IBatchReportUriGenerator
	{
		string GenerateVettingReportUri(string batchUri);
	}
}