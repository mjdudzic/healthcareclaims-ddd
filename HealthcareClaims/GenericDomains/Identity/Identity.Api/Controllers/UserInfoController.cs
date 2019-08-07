using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Identity.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OpenIddict.Validation;

namespace Identity.Api.Controllers
{
	[Route("api/userinfo")]
	[ApiController]
	public class UserInfoController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserInfoController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		//
		// GET: /api/userinfo
		[Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
		[HttpGet]
		[Produces("application/json")]
		public async Task<IActionResult> UserInfo()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return BadRequest(new OpenIdConnectResponse
				{
					Error = OpenIdConnectConstants.Errors.InvalidGrant,
					ErrorDescription = "The user profile is no longer available."
				});
			}

			var claims = new JObject
			{
				[OpenIdConnectConstants.Claims.Subject] = await _userManager.GetUserIdAsync(user)
			};

			// Note: the "sub" claim is a mandatory claim and must be included in the JSON response.

			if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Email))
			{
				claims[OpenIdConnectConstants.Claims.Email] = await _userManager.GetEmailAsync(user);
				claims[OpenIdConnectConstants.Claims.EmailVerified] = await _userManager.IsEmailConfirmedAsync(user);
			}

			if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Phone))
			{
				claims[OpenIdConnectConstants.Claims.PhoneNumber] = await _userManager.GetPhoneNumberAsync(user);
				claims[OpenIdConnectConstants.Claims.PhoneNumberVerified] = await _userManager.IsPhoneNumberConfirmedAsync(user);
			}

			claims[OpenIdConnectConstants.Claims.Role] = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

			// Note: the complete list of standard claims supported by the OpenID Connect specification
			// can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

			return Ok(claims);
		}
	}
}
