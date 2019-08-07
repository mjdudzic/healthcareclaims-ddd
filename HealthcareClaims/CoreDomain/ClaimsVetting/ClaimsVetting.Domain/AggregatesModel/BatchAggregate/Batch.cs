using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimsVetting.Domain.Events;
using ClaimsVetting.Domain.SeedWork;
using ClaimsVetting.Domain.VettingEngine;

namespace ClaimsVetting.Domain.AggregatesModel.BatchAggregate
{
	public class Batch : Entity, IAggregateRoot
	{
		private string _batchUri;
		public string GetBatchUri => _batchUri;

		private string _vettingReportUri;
		public string GetVettingReportUri => _vettingReportUri;

		private DateTime _creationDate;

		public BatchVettingStatus BatchVettingStatus { get; private set; }
		private int _batchVettingStatusId;

		public BatchFileContent GetBatchFileContent => _batchFileContent;
		private BatchFileContent _batchFileContent;

		public BatchVettingReport GetBatchVettingReport => _batchVettingReport;
		private BatchVettingReport _batchVettingReport;

		private Batch() { }

		private Batch(
			Guid id,
			string batchUri,
			BatchFileContent batchFileContent)
		{
			Id = id;
			_batchUri = batchUri;
			_creationDate = DateTime.UtcNow;
			_batchVettingStatusId = BatchVettingStatus.Started.Id;
			_batchFileContent = batchFileContent;

			AddDomainEvent(new BatchVettingStartedEvent(Id, _batchUri, _creationDate));
		}

		public static async Task<Batch> CreateAsync(
			string batchUri,
			IBatchContentService batchContentService)
		{
			var batchContent = await batchContentService.GetBatchFileContentAsync(batchUri);

			return new Batch(
				Guid.NewGuid(),
				batchUri,
				batchContent);
		}

		public async Task Vet(
			string vettingReportUri,
			IEnumerable<IBatchVettingPlugin> vettingPlugins,
			IBatchVettingReportStoreService batchVettingReportStoreService)
		{
			_vettingReportUri = vettingReportUri;

			var vettingErrors = new List<BatchVettingError>();
			var vettingWarnings = new List<BatchVettingWarning>();

			foreach (var vettingPlugin in vettingPlugins)
			{
				var vettingResult = await vettingPlugin.VetAsync(this);

				if (vettingResult.BatchVettingErrors.Any())
				{
					vettingErrors.AddRange(vettingResult.BatchVettingErrors);
				}

				if (vettingResult.BatchVettingWarnings.Any())
				{
					vettingWarnings.AddRange(vettingResult.BatchVettingWarnings);
				}
			}

			_batchVettingReport = new BatchVettingReport(
				Id,
				vettingErrors,
				vettingWarnings);

			await batchVettingReportStoreService.SaveReportFileAsync(this);

			if (vettingErrors.Any())
			{
				_batchVettingStatusId = BatchVettingStatus.Rejected.Id;
			}
			else if (vettingWarnings.Any())
			{
				_batchVettingStatusId = BatchVettingStatus.AcceptedWithWarnings.Id;
			}
			else
			{
				_batchVettingStatusId = BatchVettingStatus.Accepted.Id;
			}

			AddDomainEvent(new BatchVettingCompletedEvent(Id, _batchUri, _vettingReportUri));
		}
	}
}
