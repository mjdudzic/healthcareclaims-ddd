using System.Reflection;
using AspNet.Security.OpenIdConnect.Primitives;
using ClaimsSubmission.Api.Extensions;
using ClaimsSubmission.Api.Filters;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using ClaimsSubmission.Infrastructure.Persistence;
using ClaimsSubmission.Infrastructure.Queue.Configuration;
using ClaimsSubmission.Infrastructure.Queue.Interfaces;
using ClaimsSubmission.Infrastructure.Repositories;
using ClaimsSubmission.Infrastructure.Services;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Infrastructure.Common.Web.Consul;
using Infrastructure.Common.Web.Jaeger;
using Infrastructure.Common.Web.Minio;
using Infrastructure.Common.Web.Minio.Configuration;
using Infrastructure.Common.Web.Minio.Interfaces;
using Infrastructure.Common.Web.RabbitMq;
using Infrastructure.Common.Web.RabbitMq.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Contrib.NetCore.CoreFx;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;

namespace ClaimsSubmission.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			AddConfiguration(services);

			AddDbContexts(services);

			AddHangfire(services);

			AddServiceDiscovery(services);

			AddAuthentication(services);

			services.AddAuthorization();

			services.AddHealthChecks();

			services.AddHttpClient();

			services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

			services
				.AddRawRabbit(new RawRabbitOptions
				{
					ClientConfiguration = GetRawRabbitConfiguration()
				});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddTransient<IBatchUriGenerator, BatchUriGenerator>();
			services.AddTransient<IBatchSubmissionFeedbackStoreService, BatchSubmissionFeedbackStoreService>();
			services.AddTransient<IBatchRepository, BatchRepository>();
			services.AddTransient<IObjectsStorageService, ObjectsStorageService>();
			services.AddTransient<IMinioClientFactory, MinioClientFactory>();

			services.AddSingleton<IBusPublishService, BusPublishService>();
			services.AddSingleton<IBusSubscribeService, BusSubscribeService>();

			services.AddJaeger();
			services.AddOpenTracing();
			services.Configure<HttpHandlerDiagnosticOptions>(options =>
			{
				options.IgnorePatterns.Add(x => x.RequestUri.IsLoopback);
				options.IgnorePatterns.Add(x => x.RequestUri.LocalPath.EndsWith("/_bulk"));
			});
			services.Configure<AspNetCoreDiagnosticOptions>(options =>
			{
				options.Hosting.IgnorePatterns.Add(x => x.Request.Path == "/health");
				options.Hosting.IgnorePatterns.Add(x => x.Request.Path.Value.Contains("/dashboard"));
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseHealthChecks("/health");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			AddHangfireMiddleware(app);

			ConfigureAuth(app);

			app.UseMvc();

			app.UseRabbitMq();
		}

		private void AddServiceDiscovery(IServiceCollection services)
		{
			services.AddServiceDiscovery();
		}

		private void AddConfiguration(IServiceCollection services)
		{
			var queueConfiguration = new RabbitMqTopologyConfiguration();
			Configuration.Bind("RabbitMqTopology", queueConfiguration);
			services.AddSingleton<IRabbitMqTopologyConfiguration>(queueConfiguration);

			var objectsStorageServiceConfiguration = new ObjectsStorageServiceConfiguration();
			Configuration.Bind("ObjectsStorage", objectsStorageServiceConfiguration);
			services.AddSingleton<IObjectsStorageServiceConfiguration>(objectsStorageServiceConfiguration);
		}

		private void AddDbContexts(IServiceCollection services)
		{
			//services.AddDbContext<BatchesContext>(opt =>
			//	opt.UseInMemoryDatabase("Batches"));

			services
				.AddEntityFrameworkNpgsql()
				.AddDbContext<BatchesContext>(
					options =>
					{
						options.UseNpgsql(Configuration["DB_CONNECTION_STRING"]);
						options.EnableDetailedErrors();
					});
		}

		private void AddHangfire(IServiceCollection services)
		{
			services.AddHangfire(config =>
			{
				config.UsePostgreSqlStorage(Configuration["DB_CONNECTION_STRING"]);

				config.UseConsole();
			});
		}

		private void AddHangfireMiddleware(IApplicationBuilder app)
		{
			app.UseHangfireServer();

			app.UseHangfireDashboard("/dashboard", new DashboardOptions
			{
				Authorization = new[] { new FakeDashboardAuthFilter() }
			});
		}

		private void AddAuthentication(IServiceCollection services)
		{
			var serviceProvider = services.BuildServiceProvider();
			var env = serviceProvider.GetService<IHostingEnvironment>();

			var identityAuthority = Configuration["IDENTITY_AUTHORITY"];
			var identityAudience = Configuration["IDENTITY_AUDIENCE"];

			services
				.AddAuthentication(sharedOptions => { sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
				.AddJwtBearer(options =>
				{
					options.Authority = identityAuthority;
					options.Audience = identityAudience;
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						NameClaimType = OpenIdConnectConstants.Claims.Subject,
						RoleClaimType = OpenIdConnectConstants.Claims.Role,
						ValidateIssuer = !env.IsDevelopment()
					};
				});
		}

		private RawRabbitConfiguration GetRawRabbitConfiguration()
		{
			var section = Configuration.GetSection("RawRabbit");
			return section.Get<RawRabbitConfiguration>();
		}

		protected virtual void ConfigureAuth(IApplicationBuilder app)
		{
			app.UseAuthentication();
		}
	}
}
