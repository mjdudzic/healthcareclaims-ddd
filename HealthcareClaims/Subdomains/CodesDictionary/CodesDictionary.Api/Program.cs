using CodesDictionary.Api.Infrastructure.Persistence;
using Infrastructure.Common.Web.Metrics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CodesDictionary.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				services.GetRequiredService<CodesContext>();

				DataSeedService.SeedData(services);
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseAppMetrics()
				.UseStartup<Startup>();
	}
}
