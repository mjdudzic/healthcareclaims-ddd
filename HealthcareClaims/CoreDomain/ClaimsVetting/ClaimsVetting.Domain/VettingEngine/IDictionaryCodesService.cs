using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClaimsVetting.Domain.AggregatesModel.BatchAggregate;

namespace ClaimsVetting.Domain.VettingEngine
{
	public interface IDictionaryCodesService
	{
		Task<List<Diagnosis>> GetDiagnosesAsync(List<string> codes);
		Task<List<Procedure>> GetProceduresAsync(List<string> codes);
		Task<List<Medicine>> GetMedicinesAsync(List<string> codes);
		Task<Uri> GetCodesService();
	}
}