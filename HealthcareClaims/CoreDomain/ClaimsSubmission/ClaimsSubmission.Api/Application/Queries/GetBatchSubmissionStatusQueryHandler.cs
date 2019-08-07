using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Api.Application.Models;
using ClaimsSubmission.Infrastructure.Persistence;
using ClaimsSubmission.Infrastructure.Repositories;
using Infrastructure.Common.Web.Minio.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClaimsSubmission.Api.Application.Queries
{
	public class GetBatchSubmissionStatusQueryHandler : IRequestHandler<GetBatchSubmissionStatusQuery, BatchStatusDto>
	{
		private readonly BatchesContext _batchesContext;
		private readonly IObjectsStorageService _objectsStorageService;

		public GetBatchSubmissionStatusQueryHandler(
			BatchesContext batchesContext,
			IObjectsStorageService objectsStorageService)
		{
			_batchesContext = batchesContext;
			_objectsStorageService = objectsStorageService;
		}

		public async Task<BatchStatusDto> Handle(GetBatchSubmissionStatusQuery request, CancellationToken cancellationToken)
		{
			var batch = await _batchesContext
				.Batches
				.Include(i => i.BatchSubmissionStatus)
				.FirstOrDefaultAsync(i => i.Id == request.BatchId, cancellationToken);

			if (batch == null)
			{
				return null;
			}

			var result = new BatchStatusDto
			{
				BatchUri = batch.GetBatchUri,
				BatchStatus = batch.BatchSubmissionStatus.Name,
				FeedbackUri = await _objectsStorageService.GetObjectPresignedUrlAsync(
					BatchRepository.ObjectStorageBucket,
					batch.GetFeedbackUri,
					3600)
			};

			return result;
		}
	}
}