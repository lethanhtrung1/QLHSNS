using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Request.OverTime;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.OverTime;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class OverTimeController : ControllerBase {
		private readonly IOverTimeService _service;

		public OverTimeController(IOverTimeService service) {
			_service = service;
		}

		[HttpGet("GetTotalOverTime/{employeeId:guid}")]
		public async Task<ApiResponse<OverTimeMonthReponse>> GetTotalOverTime(Guid employeeId, int month, int year) {
			return await _service.GetOverTimeMonthReponses(employeeId, month, year);
		}

		[HttpGet("GetAllOverTime/{employeeId:guid}")]
		public async Task<ApiResponse<List<OverTimeResponse>>> GetAllOverTime(Guid employeeId) {
			return await _service.GetOverTimesByEmployeeId(employeeId);
		}

		[HttpPost("AddOverTime")]
		public async Task<ApiResponse<OverTimeResponse>> AddOverTime(CreateOverTimeRequestDto request) {
			return await _service.AddOverTime(request);
		}
	}
}
