using System.IdentityModel.Tokens.Jwt;
using AspNet.Security.OpenIdConnect.Primitives;
using Identity.Api.Infrastructure.Persistence;
using Infrastructure.Common.Web.Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;

namespace Identity.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHealthChecks();

			AddServiceDiscovery(services);

			AddDbContexts(services);

			AddIdentity(services);

			AddOpenIdConnect(services);

			AddAuthentication(services);

			AddAuthorization(services);

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHealthChecks("/health");

			app.UseAuthentication();

			app.UseMvc();

			new DataSeedService(app.ApplicationServices)
				.SeedClients()
				.GetAwaiter()
				.GetResult();
		}

		private void AddServiceDiscovery(IServiceCollection services)
		{
			//var serviceConfig = Configuration.GetServiceConfig();
			services.AddServiceDiscovery();
		}

		private void AddDbContexts(IServiceCollection services)
		{
			var connectionString = Configuration["DB_CONNECTION_STRING"];

			services
				.AddEntityFrameworkNpgsql()
				.AddDbContext<ApplicationDbContext>(options =>
				{
					options.UseNpgsql(connectionString);

					options.UseOpenIddict();
				});
		}

		private void AddIdentity(IServiceCollection services)
		{
			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.SignIn.RequireConfirmedEmail = true;
				options.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
				options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
				options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
			});
		}

		private void AddOpenIdConnect(IServiceCollection services)
		{
			services.AddOpenIddict()
				.AddCore(options =>
				{
					options.UseEntityFrameworkCore()
						.UseDbContext<ApplicationDbContext>();
				})
				.AddServer(options =>
				{
					options.UseMvc();

					options
						.EnableAuthorizationEndpoint("/connect/authorize")
						.EnableTokenEndpoint("/connect/token")
						.EnableUserinfoEndpoint("/api/userinfo")
						.EnableLogoutEndpoint("/connect/logout");

					options.RegisterScopes(OpenIdConnectConstants.Scopes.Email,
						OpenIdConnectConstants.Scopes.Profile,
						OpenIddictConstants.Scopes.Roles);

					options.AllowAuthorizationCodeFlow();
					options.AllowImplicitFlow();
					options.AllowClientCredentialsFlow();

					options.EnableRequestCaching();

					options.DisableHttpsRequirement();

					options.UseJsonWebTokens();

					options.AddEphemeralSigningKey();
				});

			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
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

		private void AddAuthorization(IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
				options.AddPolicy(
					"VipClient",
					policy => policy
						.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
						.RequireAssertion(i => i.User.HasClaim("is_vip", "1")));
			});
		}
	}
}
