using System.Collections.Generic;
using System.Linq;
using CodesDictionary.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace CodesDictionary.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CodesController : ControllerBase
	{
		private readonly CodesContext _codesContext;

		public CodesController(CodesContext codesContext)
		{
			_codesContext = codesContext;
		}

		[HttpGet("treatments")]
		public ActionResult<IEnumerable<TreatmentCode>> GetTreatments()
		{
			return _codesContext.TreatmentCodes;
		}

		[HttpGet("treatments/{code}")]
		public ActionResult<TreatmentCode> GetTreatmentByCode(string code)
		{
			return _codesContext.TreatmentCodes.FirstOrDefault(i => i.Code == code);
		}

		[HttpGet("medicines")]
		public ActionResult<IEnumerable<MedicineCode>> GetMedicines()
		{
			return _codesContext.MedicineCodes;
		}

		[HttpGet("medicines/{code}")]
		public ActionResult<MedicineCode> GetMedicineByCode(string code)
		{
			return _codesContext.MedicineCodes.FirstOrDefault(i => i.Code == code);
		}
	}
}
