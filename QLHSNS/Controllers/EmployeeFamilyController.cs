using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Request.EmployeeFamily;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.EmployeeFamily;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeFamilyController : ControllerBase {
		private readonly IEmployeeFamilyService _service;

		public EmployeeFamilyController(IEmployeeFamilyService service) {
			_service = service;
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<ApiResponse<EmployeeFamilyResponseDto>> Delete(Guid id) {
			return await _service.DeleteAsync(id);
		}

		[HttpGet("GetByEmployeeId/{id:Guid}")]
		public async Task<ApiResponse<List<EmployeeFamilyResponseDto>>> GetByEmployeeId(Guid id) {
			return await _service.GetByEmployeeIdAsync(id);
		}

		[HttpPost("Create")]
		public async Task<ApiResponse<EmployeeFamilyResponseDto>> Create(CreateEmployeeFamilyRequestDto request) {
			return await _service.CreateAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<EmployeeFamilyResponseDto>> Update(UpdateEmployeeFamilyRequestDto request) {
			return await _service.UpdateAsync(request);
		}
	}
}
