using Hangfire.Dashboard;

namespace ClaimsSubmission.Api.Filters
{
	public class FakeDashboardAuthFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			return true;
		}
	}
}