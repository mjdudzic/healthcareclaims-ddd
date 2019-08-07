using System;
using ClaimsSubmission.Api.Application.Models;
using MediatR;

namespace ClaimsSubmission.Api.Application.Queries
{
	public class GetBatchSubmissionStatusQuery : IRequest<BatchStatusDto>
	{
		public Guid BatchId { get; set; }
	}
}