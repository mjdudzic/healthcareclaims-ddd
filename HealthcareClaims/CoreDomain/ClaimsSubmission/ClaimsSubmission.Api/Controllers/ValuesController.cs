using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ClaimsSubmission.Infrastructure.Queue.Interfaces;
using Infrastructure.Common.Web.RabbitMq.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using RawRabbit;

namespace ClaimsSubmission.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IBusPublishService _busClient;

		public ValuesController(
			IHttpClientFactory httpClientFactory,
			IBusPublishService busClient)
		{
			_httpClientFactory = httpClientFactory;
			_busClient = busClient;
		}

		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<string>> Get()
		{
			//var client = _httpClientFactory.CreateClient();
			//client.PostAsync("https://webhook.site/36faebff-2334-4dc8-8301-eea8369bbb5e", new StringContent("test", Encoding.UTF8));
			return new string[] { "value1", "value2" };
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
