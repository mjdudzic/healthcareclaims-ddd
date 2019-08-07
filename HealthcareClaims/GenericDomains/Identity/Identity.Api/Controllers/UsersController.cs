using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class UsersController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(User.Identity.Name);
		}

		[HttpGet("vip")]
		[Authorize(Policy = "VipClient")]
		public IActionResult GetVip()
		{
			return Ok(User.Identity.Name);
		}
	}
}
