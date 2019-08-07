using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;
using ClaimsVetting.Domain.VettingEngine;
using ClaimsVetting.Infrastructure.Adapters;
using Consul;
using Infrastructure.Common.Web.Consul;

namespace ClaimsVetting.Infrastructure.Services
{
	public class DictionaryCodesService : IDictionaryCodesService
	{
		private readonly IDictionaryCodesOptions _options;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConsulClient _consulClient;

		public DictionaryCodesService(
			IDictionaryCodesOptions options,
			IHttpClientFactory httpClientFactory,
			IConsulClient consulClient)
		{
			_options = options;
			_httpClientFactory = httpClientFactory;
			_consulClient = consulClient;
		}

		public async Task<List<Diagnosis>> GetDiagnosesAsync(List<string> codes)
		{
			var client = _httpClientFactory.CreateClient();

			var result = new List<Diagnosis>();

			var adapter = new TreatmentCodeAdapter(client);

			foreach (var code in codes)
			{
				var diagnosis = await adapter.ToDiagnosisAsync(
					_options.Endpoint,
					code);

				if (diagnosis != null)
				{
					result.Add(diagnosis);
				}	
			}

			return result;
		}

		public async Task<List<Procedure>> GetProceduresAsync(List<string> codes)
		{
			var client = _httpClientFactory.CreateClient();

			var result = new List<Procedure>();

			var adapter = new TreatmentCodeAdapter(client);

			foreach (var code in codes)
			{
				var procedure = await adapter.ToProcedureAsync(
					_options.Endpoint,
					code);

				if (procedure != null)
				{
					result.Add(procedure);
				}
			}

			return result;
		}

		public async Task<List<Medicine>> GetMedicinesAsync(List<string> codes)
		{
			var client = _httpClientFactory.CreateClient();

			var apiEndpoint = await GetCodesService();

			var result = new List<Medicine>();

			var adapter = new TreatmentCodeAdapter(client);

			foreach (var code in codes)
			{
				var medicine = await adapter.ToMedicineAsync(
					apiEndpoint?.ToString() ?? _options.Endpoint,
					code);

				if (medicine != null)
				{
					result.Add(medicine);
				}
			}

			return result;
		}

		public async Task<Uri> GetCodesService()
		{
			var query = await _consulClient.Catalog.Service("codesdictionary");
			var service = query.Response.FirstOrDefault();

			if (service == null)
			{
				return null;
			}

			return new Uri($"http://{service.ServiceAddress}:{service.ServicePort}");
			//return new Uri($"{service.Address}");
		}
	}
}