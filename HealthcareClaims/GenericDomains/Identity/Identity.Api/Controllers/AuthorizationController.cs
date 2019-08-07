/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/openiddict/openiddict-core for more information concerning
 * the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Identity.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Mvc.Internal;
using OpenIddict.Server;

namespace Identity.Api.Controllers
{
	public class AuthorizationController : Controller
	{
		private readonly OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;
		private readonly IOptions<IdentityOptions> _identityOptions;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;

		public AuthorizationController(
			OpenIddictApplicationManager<OpenIddictApplication> applicationManager,
			IOptions<IdentityOptions> identityOptions,
			SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager,
			IConfiguration configuration)
		{
			_applicationManager = applicationManager;
			_identityOptions = identityOptions;
			_signInManager = signInManager;
			_userManager = userManager;
			_configuration = configuration;
		}

		[HttpPost("~/connect/token"), Produces("application/json")]
		public async Task<IActionResult> Exchange([ModelBinder(typeof(OpenIddictMvcBinder))] OpenIdConnectRequest request)
		{
			return await HandleAuthorizationFlow(request);
		}

		private async Task<IActionResult> HandleAuthorizationFlow(OpenIdConnectRequest request)
		{
			if (request.IsClientCredentialsGrantType())
			{
				return await HandleClientCredentialsGrantTypeFlow(request);
			}

			return BadRequest(new OpenIdConnectResponse
			{
				Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
				ErrorDescription = "The specified grant type is not supported."
			});
		}

		private async Task<IActionResult> HandleClientCredentialsGrantTypeFlow(OpenIdConnectRequest request)
		{
			// Note: the client credentials are automatically validated by OpenIddict:
			// if client_id or client_secret are invalid, this action won't be invoked.

			var application = await _applicationManager.FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);
			if (application == null)
			{
				return BadRequest(new OpenIdConnectResponse
				{
					Error = OpenIdConnectConstants.Errors.InvalidClient,
					ErrorDescription = "The client application was not found in the database."
				});
			}

			// Create a new authentication ticket.
			var ticket = CreateClientCredentialsTicket(application);

			return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
		}

		private AuthenticationTicket CreateClientCredentialsTicket(
			OpenIddictApplication application)
		{
			// Create a new ClaimsIdentity containing the claims that
			// will be used to create an id_token, a token or a code.

			var identity = new ClaimsIdentity(
				OpenIddictServerDefaults.AuthenticationScheme,
				OpenIdConnectConstants.Claims.Name,
				OpenIdConnectConstants.Claims.Role);

			// Use the client_id as the subject identifier.
			identity.AddClaim(OpenIdConnectConstants.Claims.Subject, application.ClientId,
				OpenIdConnectConstants.Destinations.AccessToken,
				OpenIdConnectConstants.Destinations.IdentityToken);

			identity.AddClaim(OpenIdConnectConstants.Claims.Name, application.DisplayName,
				OpenIdConnectConstants.Destinations.AccessToken,
				OpenIdConnectConstants.Destinations.IdentityToken);

			// Create a new authentication ticket holding the user identity.
			var ticket = new AuthenticationTicket(
				new ClaimsPrincipal(identity),
				new AuthenticationProperties(),
				OpenIddictServerDefaults.AuthenticationScheme);

			SetAppCustomClaimsForClientCredentialsTicket(application, identity);

			ticket.SetResources(_configuration["IDENTITY_AUDIENCE"]);

			return ticket;
		}

		private void SetAppCustomClaimsForClientCredentialsTicket(
			OpenIddictApplication application,
			ClaimsIdentity identity)
		{
			if (string.IsNullOrWhiteSpace(application.Permissions))
			{
				return;
			}

			var permissions = JsonConvert.DeserializeObject<string[]>(application.Permissions);
			if (permissions == null || permissions.Any(i => i == $"{OpenIddictConstants.Permissions.Prefixes.Scope}is_vip") == false)
			{
				return;
			}

			identity.AddClaim("is_vip", "1",
				OpenIdConnectConstants.Destinations.AccessToken,
				OpenIdConnectConstants.Destinations.IdentityToken);
		}
	}
}