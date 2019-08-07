using System.Collections.Generic;
using InsuredMembers.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace InsuredMembers.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MembersController : ControllerBase
	{
		private readonly MembersContext _membersContext;

		public MembersController(MembersContext membersContext)
		{
			_membersContext = membersContext;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Member>> Get()
		{
			return _membersContext.Members;
		}

		[HttpPost]
		public IActionResult Post(Member member)
		{
			return Accepted();
		}

		[HttpPost("{id}")]
		public IActionResult Delete(int id)
		{
			return Accepted();
		}
	}
}