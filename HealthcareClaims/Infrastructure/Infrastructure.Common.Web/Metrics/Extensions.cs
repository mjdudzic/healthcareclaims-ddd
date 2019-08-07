using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Health;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Common.Web.Metrics
{
	public static class Extensions
	{
		public static IWebHostBuilder UseAppMetrics(this IWebHostBuilder webHostBuilder)
		{
			return webHostBuilder
				.ConfigureMetricsWithDefaults((context, builder) =>
				{
					var metricsOptions = new MetricsOptions();
					context.Configuration.GetSection("Metrics").Bind(metricsOptions);

					if (!metricsOptions.Enabled)
					{
						return;
					}

					builder.Report.ToInfluxDb(o =>
					{
						o.InfluxDb.Database = metricsOptions.Database;
						o.InfluxDb.BaseUri = new Uri(metricsOptions.InfluxUrl);
						o.InfluxDb.CreateDataBaseIfNotExists = true;
						o.FlushInterval = TimeSpan.FromSeconds(metricsOptions.Interval);
					});
				})
				.UseHealth()
				.UseHealthEndpoints()
				.UseMetricsWebTracking()
				.UseMetrics();
		}
	}
}