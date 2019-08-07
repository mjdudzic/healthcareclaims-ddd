using System;
using System.Threading;
using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;
using ClaimsVetting.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClaimsVetting.Infrastructure.Persistence
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

		public DbSet<BatchVettingStatus> BatchVettingStatuses { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Batch>(entity =>
				{
					entity.ToTable("batches");

					entity.HasKey(i => i.Id);
					entity.Property(i => i.Id)
						.HasColumnName("id")
						.ValueGeneratedNever();

					entity.Property<int>("BatchVettingStatusId")
						.HasColumnName("batch_vetting_status_id")
						.IsRequired();

					entity.Property<string>("BatchUri")
						.HasColumnName("batch_uri")
						.HasMaxLength(2000)
						.IsRequired();

					entity.Property<string>("VettingReportUri")
						.HasColumnName("vetting_report_uri")
						.HasMaxLength(2000)
						.IsRequired();

					entity.Property<DateTime>("CreationDate")
						.HasColumnName("creation_date")
						.IsRequired();

					entity.HasOne(i => i.BatchVettingStatus)
						.WithMany()
						.HasForeignKey("BatchVettingStatusId");
				});

			modelBuilder
				.Entity<BatchVettingStatus>(entity =>
				{
					entity.ToTable("batch_vetting_statuses");

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
