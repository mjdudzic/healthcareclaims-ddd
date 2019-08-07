using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;

namespace HealthcareClaims.ApiGateway
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = CreateWebHostBuilder(args);

			builder.ConfigureAppConfiguration(
					i => i.AddJsonFile("ocelot.json", false, true));

			builder.Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
