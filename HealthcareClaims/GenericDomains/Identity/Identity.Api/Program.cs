using System;
using System.IO;
using System.Threading;
using DbUp;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Identity.Api
{
	public class Program
	{
		private const int ProbesCountMax = 10;

		public static void Main(string[] args)
		{
			BuildDatabase();
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();

		private static void BuildDatabase()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();

			var configuration = builder.Build();
			var connectionString = configuration.GetSection("DB_CONNECTION_STRING").Value;

			CheckDbExists(connectionString, 1);

			var result =
				DeployChanges.To
					.PostgresqlDatabase(connectionString)
					.WithScriptsFromFileSystem(Path.Combine(Directory.GetCurrentDirectory(), "Database/ChangeScripts"))
					.LogToConsole()
					.WithVariablesDisabled()
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
	}
}
