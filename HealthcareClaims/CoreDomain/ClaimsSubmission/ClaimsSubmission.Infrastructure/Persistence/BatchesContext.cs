using System;
using System.Threading;
using System.Threading.Tasks;
using ClaimsSubmission.Domain.AggregatesModel.BatchAggregate;
using ClaimsSubmission.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClaimsSubmission.Infrastructure.Persistence
{
	public class BatchesContext : DbContext
	{
		private readonly IMediator _mediator;

		public BatchesContext(DbContextOptions options, IMediator mediator)
			: base(options)
		{
			_mediator = mediator;
		}

		public DbSet<Batch> Batches { get; set; }

		public DbSet<BatchSubmissionStatus> BatchSubmissionStatuses { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Batch>(entity =>
				{
					entity.ToTable("batches");
					entity.Ignore(b => b.DomainEvents);

					entity.HasKey(i => i.Id);
					entity.Property(i => i.Id)
						.HasColumnName("id")
						.ValueGeneratedNever();

					entity.Property<int>("BatchSubmissionStatusId")
						.HasColumnName("batch_submission_status_id")
						.IsRequired();

					entity.Property<string>("BatchUri")
						.HasColumnName("batch_uri")
						.HasMaxLength(2000)
						.IsRequired();

					entity.Property<string>("FeedbackUri")
						.HasColumnName("feedback_uri")
						.HasMaxLength(2000);

					entity.Property<DateTime>("CreationDate")
						.HasColumnName("creation_date")
						.IsRequired();

					entity.HasOne(i => i.BatchSubmissionStatus)
						.WithMany()
						.HasForeignKey("BatchSubmissionStatusId");
				});

			modelBuilder
				.Entity<BatchSubmissionStatus>(entity =>
				{
					entity.ToTable("batch_submission_statuses");

					entity.HasKey(i => i.Id);
					entity.Property(i => i.Id)
						.HasColumnName("id")
						.ValueGeneratedNever()
						.IsRequired();

					entity.Property(o => o.Name)
						.HasColumnName("name")
						.HasMaxLength(200)
						.IsRequired();
				});
		}

		public async Task SaveEntitiesAsync(CancellationToken cancellationToken = default)
		{
			await _mediator.DispatchDomainEventsAsync(this);

			await base.SaveChangesAsync(cancellationToken);
		}
	}
}
