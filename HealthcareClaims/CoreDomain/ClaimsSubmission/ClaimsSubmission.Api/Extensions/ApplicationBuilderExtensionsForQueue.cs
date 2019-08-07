using System;
using System.Threading;
using ClaimsSubmission.Api.Application.Messages.Events;
using ClaimsSubmission.Infrastructure.Queue.Configuration;
using Infrastructure.Common.Web.RabbitMq.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClaimsSubmission.Api.Extensions
{
	public static class ApplicationBuilderExtensionsForQueue
	{
		private const int ProbesCountMax = 5;
		private static int _probe;

		public static RabbitMqTopologyConfiguration Configuration { get; set; }
		public static IBusSubscribeService BusSubscribeService { get; set; }

		public static IApplicationBuilder UseRabbitMq(this IApplicationBuilder app)
		{
			try
			{
				var configService = app.ApplicationServices.GetService<IConfiguration>();
				Configuration = new RabbitMqTopologyConfiguration();
				configService.Bind("RabbitMqTopology", Configuration);

				BusSubscribeService =  app.ApplicationServices.GetService<IBusSubscribeService>();

				var life = app.ApplicationServices.GetService<IApplicationLifetime>();

				life.ApplicationStarted.Register(() =>
				{
					try
					{
						OnStarted();
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						throw;
					}
				});

				return app;
			}
			catch (Exception)
			{
				if (_probe == ProbesCountMax)
					throw;

				Console.WriteLine($"Probe {_probe}/{ProbesCountMax}: Checking rabbitClient availability...");
				Thread.Sleep(_probe * 1000 * 5);
				_probe++;

				return UseRabbitMq(app);
			}

		}

		private static void OnStarted()
		{
			BusSubscribeService.SubscribeForEvent<BatchSubmissionCompletedEvent>(
				Configuration.ExchangeName,
				Configuration.RoutingKey,
				Configuration.BatchesSubmittedQueueName);
		}
	}
}