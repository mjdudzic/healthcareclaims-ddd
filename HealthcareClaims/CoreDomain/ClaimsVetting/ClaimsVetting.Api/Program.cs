using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;
using ClaimsVetting.Infrastructure.Persistence;
using DbUp;
using Infrastructure.Common.Web.Metrics;
using Infrastructure.Common.Web.Serilog;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace ClaimsVetting.Api
{
	public class Program
	{
		private const int ProbesCountMax = 10;

		private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables()
			.Build();

		public static HttpClient HttpClient = new HttpClient();

		public static void Main(string[] args)
		{
			try
			{
				BuildLogger();
				BuildDatabase();

				var host = CreateWebHostBuilder(args).Build();

				using (var scope = host.Services.CreateScope())
				{
					var services = scope.ServiceProvider;
					services.GetRequiredService<BatchesContext>();

					Initialize(services);
				}

				host.Run();
			}
			catch (Exception e)
			{
				Log.Fatal(e, "Host terminated unexpectedly");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseSerilog()
				.UseStartup<Startup>();

		private static void BuildLogger()
		{
			var elasticSearchUrl = Configuration.GetSection("ElasticConfiguration:Url").Value;

			//CheckElasticSearchResponds(elasticSearchUrl, 1);

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(Configuration)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
				{
					AutoRegisterTemplate = true,
					MinimumLogEventLevel = LogEventLevel.Information
				})
				.WriteTo.OpenTracing()
				.CreateLogger();
		}

		private static void BuildDatabase()
		{
			var connectionString = Configuration.GetSection("DB_CONNECTION_STRING").Value;

			CheckDbExists(connectionString, 1);

			var result =
				DeployChanges.To
					.PostgresqlDatabase(connectionString)
					.WithScriptsFromFileSystem(Path.Combine(Directory.GetCurrentDirectory(), "Database/ChangeScripts"))
					.LogToConsole()
					.Build()
					.PerformUpgrade();

			if (!result.Successful)
			{
				Console.WriteLine(result.Error);
			}
		}

		private static void CheckDbExists(string connectionString, int probeCount)
		{
			try
			{
				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: Checking db connection...");

				EnsureDatabase.For.PostgresqlDatabase(connectionString);

				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: db connection OK");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: Cannot connect to db: {e.Message}");
				if (probeCount == ProbesCountMax)
					throw;

				var waitTime = probeCount * 1000 * 5;

				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: Wait {waitTime / 1000} seconds and try again ...");

				Thread.Sleep(waitTime);

				CheckDbExists(connectionString, probeCount + 1);
			}
		}

		private static void Initialize(IServiceProvider serviceProvider)
		{
			using (var context = new BatchesContext(
				serviceProvider.GetRequiredService<DbContextOptions<BatchesContext>>(),
				serviceProvider.GetRequiredService<IMediator>()))
			{
				if (context.BatchVettingStatuses.Any())
				{
					return;
				}

				context.AddRange(
					BatchVettingStatus.Started,
					BatchVettingStatus.Rejected,
					BatchVettingStatus.Accepted,
					BatchVettingStatus.AcceptedWithWarnings);

				context.SaveChanges();
			}
		}

		private static void CheckElasticSearchResponds(string url, int probeCount)
		{
			try
			{
				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: Checking ElasticSearch...");

				var response = HttpClient.GetAsync(url).GetAwaiter().GetResult();
				response.EnsureSuccessStatusCode();

				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: ElasticSearch connection OK");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: Cannot connect to ElasticSearch: {e.Message}");
				if (probeCount == ProbesCountMax)
					throw;

				var waitTime = probeCount * 1000 * 5;

				Console.WriteLine($"Probe {probeCount}/{ProbesCountMax}: Wait {waitTime / 1000} seconds and try again ...");

				Thread.Sleep(waitTime);

				CheckElasticSearchResponds(url, probeCount + 1);
			}
		}
	}
}
