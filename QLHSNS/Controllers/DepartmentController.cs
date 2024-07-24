using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Department;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Department;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentController : ControllerBase {
		private readonly IDepartmentService _service;

		public DepartmentController(IDepartmentService service) {
			_service = service;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<DepartmentResponseDto>> GetById(Guid id) {
			return await _service.GetByIdAsync(id);
		}

		[HttpPost("GetList")]
		public async Task<ApiResponse<PagedResult<DepartmentResponseDto>>> GetList(PagingRequestBase request) {
			return await _service.GetDepartmentsAsync(request);
		}

		[HttpPost("Create")]
		public async Task<ApiResponse<DepartmentResponseDto>> Create(CreateDepartmentRequestDto request) {
			return await _service.CreateDepartmentAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<DepartmentResponseDto>> Update(UpdateDepartmentRequestDto request) {
			return await _service.UpdateDepartmentAsync(request);
		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<DepartmentResponseDto>> EnableDepartment(Guid id) {
			return await _service.EnableDepartmentAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<DepartmentResponseDto>> DisableDepartment(Guid id) {
			return await _service.DisableDepartmentAsync(id);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id) {
			var result = await _service.DeleteDepartmentAsync(id);
			return result ? Ok() : BadRequest();
		}

		[HttpPost("AddDepartmentJobTitle")]
		public async Task<ApiResponse<List<DepartmentJobTitleResponseDto>>> AddDepartmentJobTitle(DepartmentJobTitleRequestDto request) {
			return await _service.AddDepartmentJobTitleAsync(request);
		}

		[HttpPut("UpdateDepartmentJobTitle")]
		public async Task<ApiResponse<List<DepartmentJobTitleResponseDto>>> UpdateDepartmentJobTitle(DepartmentJobTitleRequestDto request) {
			return await _service.UpdateDepartmentJobTitleAsync(request);
		}

		[HttpGet("GetAllDepartment/{status:int}")]
		public async Task<ApiResponse<List<DepartmentBaseResponseDto>>> GetAll(int status) {
			return await _service.GetAllAsync(status);
		}
	}
}
