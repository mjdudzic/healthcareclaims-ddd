using CodesDictionary.Api.Infrastructure.Persistence;
using Infrastructure.Common.Web.Consul;
using Infrastructure.Common.Web.Jaeger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Contrib.NetCore.CoreFx;

namespace CodesDictionary.Api
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
			services.AddHealthChecks();

			AddServiceDiscovery(services);

			AddDbContexts(services);

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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

			app.UseMvc();
		}

		private void AddServiceDiscovery(IServiceCollection services)
		{
			services.AddServiceDiscovery();
		}

		private void AddDbContexts(IServiceCollection services)
		{
			services.AddDbContext<CodesContext>(opt =>
				opt.UseInMemoryDatabase("Codes"));
		}
	}
}
