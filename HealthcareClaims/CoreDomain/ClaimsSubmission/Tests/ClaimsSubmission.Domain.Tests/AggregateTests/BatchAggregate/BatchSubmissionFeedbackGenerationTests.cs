using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using ClaimsSubmission.Domain.Events;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Domain.Tests.AggregateTests.BatchAggregate
{
	public class BatchSubmissionFeedbackGenerationTests
	{
		[Fact]
		public async Task GivenBatch_WhenBatchSubmissionFeedbackCreated_ThenBatchSubmissionFeedbackGeneratedEventIsAdded()
		{
			// Arrange
			var batch = GetTestBatchInstance();
			var batchSubmissionFeedbackStoreService = Substitute
				.For<IBatchSubmissionFeedbackStoreService>();

			// Act
			await batch.GenerateSubmissionFeedbackAsync(
				Guid.NewGuid().ToString(),
				batchSubmissionFeedbackStoreService);

			// Assert
			batch.DomainEvents
				.Any(i => i.GetType() == typeof(BatchSubmissionFeedbackGeneratedEvent))
				.Should()
				.BeTrue();
		}

		private Batch GetTestBatchInstance()
		{
			Batch batch;
			using (var stream = new MemoryStream())
			{
				batch = new Batch(
					Guid.NewGuid(),
					Guid.NewGuid().ToString(),
					stream);
			}

			batch.BatchFileContent = new BatchFileContent { Claims = new List<Claim>() };
			return batch;
		}

	}
}
