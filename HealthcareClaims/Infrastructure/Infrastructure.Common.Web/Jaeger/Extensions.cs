using System;
using System.Collections.Generic;
using System.Reflection;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using OpenTracing.Util;

namespace Infrastructure.Common.Web.Jaeger
{
	public static class TracingExtensions
	{
		public static IServiceCollection AddJaeger(
			this IServiceCollection services)
		{
			var options = GetJaegerOptions(services);

			if (!options.Enabled)
			{
				var defaultTracer = CreateDefaultTracer();
				services.AddSingleton(defaultTracer);
				return services;
			}

			services.AddSingleton<ITracer>(sp =>
			{
				var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

				var reporter = new RemoteReporter.Builder()
					.WithSender(new UdpSender(options.UdpHost, options.UdpPort, options.MaxPacketSize))
					.WithLoggerFactory(loggerFactory)
					.Build();

				var sampler = GetSampler(options);

				var serviceName = sp.GetRequiredService<IHostingEnvironment>().ApplicationName;

				var tracer = new Tracer.Builder(serviceName)
					.WithReporter(reporter)
					.WithSampler(sampler)
					.Build();

				GlobalTracer.Register(tracer);

				return tracer;
			});

			return services;
		}

		public static IScope StartServerSpan(ITracer tracer, IDictionary<string, string> headers, string operationName)
		{
			ISpanBuilder spanBuilder;
			try
			{
				ISpanContext parentSpanCtx = tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(headers));

				spanBuilder = tracer.BuildSpan(operationName);
				if (parentSpanCtx != null)
				{
					spanBuilder = spanBuilder.AsChildOf(parentSpanCtx);
				}
			}
			catch (Exception)
			{
				spanBuilder = tracer.BuildSpan(operationName);
			}

			return spanBuilder.WithTag(Tags.SpanKind, Tags.SpanKindConsumer).StartActive(true);
		}

		private static JaegerOptions GetJaegerOptions(IServiceCollection services)
		{
			using (var serviceProvider = services.BuildServiceProvider())
			{
				var configuration = serviceProvider.GetService<IConfiguration>();
				var options = new JaegerOptions();
				configuration.Bind("Jaeger", options);

				return options;
			}
		}

		private static ISampler GetSampler(JaegerOptions options)
		{
			switch (options.Sampler)
			{
				case "const": return new ConstSampler(true);
				case "rate": return new RateLimitingSampler(options.MaxTracesPerSecond);
				case "probabilistic": return new ProbabilisticSampler(options.SamplingRate);
				default: return new ConstSampler(true);
			}
		}

		public static ITracer CreateDefaultTracer()
			=> new Tracer.Builder(Assembly.GetEntryAssembly().FullName)
				.WithReporter(new NoopReporter())
				.WithSampler(new ConstSampler(false))
				.Build();
	}
}