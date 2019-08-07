using System;
using System.IO;
using System.Linq;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using ClaimsSubmission.Domain.Events;
using FluentAssertions;
using Xunit;

namespace Domain.Tests.AggregateTests.BatchAggregate
{
	public class BatchCreationTests
	{
		[Fact]
		public void GivenBatchValues_WhenBatchInstanceCreated_ThenBatchSubmissionStartedEventIsAdded()
		{
			// Arrange
			var batchId = Guid.NewGuid();
			var batchUri = Guid.NewGuid().ToString();

			// Act
			Batch batch;
			using (var stream = new MemoryStream())
			{
				batch = new Batch(
					batchId,
					batchUri,
					stream);
			}

			// Assert
			batch.DomainEvents
				.Any(i => i.GetType() == typeof(BatchSubmissionStartedEvent))
				.Should()
				.BeTrue();
		}

	}
}
