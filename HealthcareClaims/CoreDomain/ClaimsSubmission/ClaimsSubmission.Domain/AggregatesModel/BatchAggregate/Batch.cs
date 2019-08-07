using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClaimsSubmission.Domain.Events;
using ClaimsSubmission.Domain.SeedWork;

namespace ClaimsSubmission.Domain.AggregatesModel.BatchAggregate
{
	public class Batch : Entity, IAggregateRoot
	{
		private string _batchUri;
		public string GetBatchUri => _batchUri;

		private string _feedbackUri;
		public string GetFeedbackUri => _feedbackUri;

		private DateTime _creationDate;

		public BatchSubmissionStatus BatchSubmissionStatus { get; private set; }
		private int _batchSubmissionStatusId;

		private readonly Stream _bachFileContentStream;
		public Stream GetBachFileContentStream => _bachFileContentStream;

		private BatchFileContent _batchFileContent;
		public BatchFileContent BatchFileContent
		{
			set => _batchFileContent = value;
		}

		private BatchSubmissionFeedback _batchSubmissionFeedback;
		public BatchSubmissionFeedback GetBatchSubmissionFeedback => _batchSubmissionFeedback;
		public BatchSubmissionFeedback BatchSubmissionFeedback
		{
			set => _batchSubmissionFeedback = value;
		}

		private Batch() { }

		public Batch(
			Guid batchId,
			string batchUri,
			Stream bachFileContentStream)
		{
			Id = batchId;
			_batchUri = batchUri;
			_bachFileContentStream = bachFileContentStream;
			_creationDate = DateTime.UtcNow;
			_batchSubmissionStatusId = BatchSubmissionStatus.Started.Id;

			AddDomainEvent(new BatchSubmissionStartedEvent(Id, _batchUri, _creationDate));
		}

		public async Task GenerateSubmissionFeedbackAsync(
			string feedbackUri,
			IBatchSubmissionFeedbackStoreService batchSubmissionFeedbackStoreService)
		{
			_feedbackUri = feedbackUri;
			_batchSubmissionStatusId = BatchSubmissionStatus.InProgress.Id;

			var errors = new List<BatchError>();

			if (string.IsNullOrWhiteSpace(_batchFileContent.BatchNumber))
			{
				errors.Add(new BatchError(
					FeedbackFieldLevelType.Batch,
					nameof(_batchFileContent.BatchNumber),
					"ERR_BatchNumber",
					"Field cannot be empty"));
			}

			var validatedClaims = new List<Claim>();

			foreach (var claim in _batchFileContent.Claims)
			{
				var existingClaim = validatedClaims.FirstOrDefault(i => i.Equals(claim));
				if (existingClaim != null)
				{
					errors.Add(new BatchError(
						FeedbackFieldLevelType.Claim,
						$"Claim_{claim.ClaimNumber}",
						"ERR_ClaimDuplicate",
						$"Claim is a duplicate of {existingClaim.ClaimNumber}"));
				}

				validatedClaims.Add(claim);
			}

			_batchSubmissionFeedback = new BatchSubmissionFeedback(Id, errors);

			await batchSubmissionFeedbackStoreService.SaveFeedbackFile(this);

			AddDomainEvent(new BatchSubmissionFeedbackGeneratedEvent(Id, feedbackUri, _batchSubmissionFeedback.HasErrors));
		}

		public void Accept()
		{
			_batchSubmissionStatusId = BatchSubmissionStatus.Accepted.Id;

			AddDomainEvent(new BatchSubmissionAcceptedEvent(Id));
		}

		public void Reject()
		{
			_batchSubmissionStatusId = BatchSubmissionStatus.Rejected.Id;

			AddDomainEvent(new BatchSubmissionRejectedEvent(Id));
		}
	}
}
