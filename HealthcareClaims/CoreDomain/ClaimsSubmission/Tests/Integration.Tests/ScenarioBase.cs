using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Integration.Tests
{
	public class ScenarioBase
	{
		public TestServer CreateServer()
		{
			var path = Assembly.GetAssembly(typeof(ScenarioBase))
				.Location;

			var hostBuilder = new WebHostBuilder()
				.UseContentRoot(Path.GetDirectoryName(path))
				.ConfigureAppConfiguration(cb =>
				{
					cb.AddJsonFile("appsettings.json", optional: false)
					.AddEnvironmentVariables();
				})
				.UseStartup<TestStartup>();

			var testServer = new TestServer(hostBuilder);

			return testServer;
		}
	}
}
