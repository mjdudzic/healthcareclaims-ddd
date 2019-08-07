using System;
using System.Threading.Tasks;
using ClaimsSubmission.Api.Application.Commands;
using ClaimsSubmission.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsSubmission.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class BatchesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BatchesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetBatchStatus(Guid id)
		{
			return Ok(await _mediator.Send(new GetBatchSubmissionStatusQuery { BatchId = id }));
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromForm] SubmitBatchCommand command)
		{
			var commandResult = await _mediator.Send(command);

			return AcceptedAtAction(
				nameof(GetBatchStatus),
				new {id = commandResult},
				null);
		}
	}
}