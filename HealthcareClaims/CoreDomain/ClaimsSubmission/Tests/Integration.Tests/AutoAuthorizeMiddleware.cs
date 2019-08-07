using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Http;

namespace Integration.Tests
{
	internal class AutoAuthorizeMiddleware
	{
		public const string IdentityId = "9e3163b9-1ae6-4652-9dc6-7898ab7b7a00";

		private readonly RequestDelegate _next;

		public AutoAuthorizeMiddleware(RequestDelegate rd)
		{
			_next = rd;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			var identity = new ClaimsIdentity();

			identity.AddClaim(new Claim(OpenIdConnectConstants.Claims.Subject, "test"));
			identity.AddClaim(new Claim(OpenIdConnectConstants.Claims.Name, "test"));
			identity.AddClaim(new Claim(OpenIdConnectConstants.Claims.Audience, "healthcareclaims.apps"));
			identity.AddClaim(new Claim(OpenIdConnectConstants.Claims.Issuer, "http://localhost:7001/"));

			httpContext.User.AddIdentity(identity);

			await _next.Invoke(httpContext);
		}
	}
}
