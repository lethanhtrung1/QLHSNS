using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Employee;
using QLHSNS.DTOs.Request.EmployeeRequestDto;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Employee;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase {
		private readonly IEmployeeService _service;

		public EmployeeController(IEmployeeService service) {
			_service = service;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<EmployeeResponseDto>> GetEmployeeById(Guid id) {
			return await _service.GetEmployeeByIdAsync(id);
		}

		[HttpPost("CreateEmployeeAsset")]
		public async Task<ApiResponse<EmployeeAssetResponseDto>> CreateEmployeeAsset(EmployeeAssetRequestDto request) {
			return await _service.CreateEmployeeAssetsAsync(request);
		}

		[HttpPost("GetEmployees")]
		public async Task<ApiResponse<PagedResult<EmployeeResponseDto>>> GetEmployees(PagingRequestBase request) {
			return await _service.GetEmployeesAsync(request);
		}

		[HttpPost("CreateEmployee")]
		public async Task<ApiResponse<EmployeeResponseDto>> CreateEmployee(CreateEmployeeRequestDto request) {
			return await _service.CreateNewEmployeeAsync(request);
		}

		[HttpPut("UpdateEmployee")]
		public async Task<ApiResponse<EmployeeResponseDto>> UpdateEmloyee(UpdateEmployeeRequestDto request) {
			return await _service.UpdateEmployeeAsyns(request);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<IActionResult> DeleteEmployee(Guid id) {
			var result = await _service.DeleteEmployeeAsync(id);
			return result ? Ok() : BadRequest();
		}

		[HttpPost("SearchEmployee")]
		public async Task<ApiResponse<PagedResult<EmployeeResponseDto>>> Search(EmployeePagingRequestDto request) {
			return await _service.SearchAllEmployeeAsync(request);
		}

		[HttpGet("GetAssetByEmployeeId/{id:Guid}")]
		public async Task<ApiResponse<EmployeeAssetResponseDto>> GetAssetByEmployeeId(Guid id) {
			return await _service.GetAssetByEmployeeId(id);
		}
	}
}
