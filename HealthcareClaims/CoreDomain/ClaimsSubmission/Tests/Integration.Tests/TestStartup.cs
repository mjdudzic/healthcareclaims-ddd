using ClaimsSubmission.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Integration.Tests
{
	public class TestStartup : Startup
	{
		public TestStartup(IConfiguration env) : base(env)
		{
		}

		protected override void ConfigureAuth(IApplicationBuilder app)
		{
			app.UseMiddleware<AutoAuthorizeMiddleware>();
		}
	}
}