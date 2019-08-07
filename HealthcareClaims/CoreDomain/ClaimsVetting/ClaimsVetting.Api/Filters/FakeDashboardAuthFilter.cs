using Hangfire.Dashboard;

namespace ClaimsVetting.Api.Filters
{
	public class FakeDashboardAuthFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			return true;
		}
	}
}