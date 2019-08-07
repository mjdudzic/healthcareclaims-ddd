using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ClaimsSubmission.Api.Application.Commands
{
	public class SubmitBatchCommand : IRequest<Guid>
	{
		public Guid HealthcareProviderId { get; set; }
		public IFormFile BatchJsonFile { get; set; }
	}
}
