using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimsVetting.Domain.VettingEngine;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsVetting.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly IDictionaryCodesService _dictionaryCodesService;

		public ValuesController(
			IDictionaryCodesService dictionaryCodesService)
		{
			_dictionaryCodesService = dictionaryCodesService;
		}

		// GET api/values
		[HttpGet]
		public async Task<ActionResult<IEnumerable<string>>> Get()
		{
			var service = await _dictionaryCodesService.GetCodesService();
			return new string[] { service?.AbsoluteUri ?? "", "value2" };
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return "value";
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
