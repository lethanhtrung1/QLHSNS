using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.JobTitle;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.JobTitle;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class JobTitleController : ControllerBase {
		private readonly IJobTitleService _jobTitleService;
		public JobTitleController(IJobTitleService jobTitleService) {
			_jobTitleService = jobTitleService;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<JobTitleResponseDto>> GetById(Guid id) {
			return await _jobTitleService.GetByIdAsync(id);
		}

		[HttpGet("GetByDepartmentId/{id:Guid}")]
		public async Task<ApiResponse<List<JobTitleResponseDto>>> GetByDepartmentId(Guid id) {
			return await _jobTitleService.GetByDepartmentIdAsync(id);
		}

		[HttpPost("GetAll")]
		public async Task<ApiResponse<PagedResult<JobTitleResponseDto>>> GetAll(PagingRequestBase request) {
			return await _jobTitleService.GetAllAsync(request);
		}

		[HttpPost("Create")]
		public async Task<ApiResponse<JobTitleResponseDto>> Create([FromBody] CreateJobTitleRequestDto request) {
			return await _jobTitleService.CreateAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<JobTitleResponseDto>> Update([FromBody] UpdateJobTitleRequestDto request) {
			return await _jobTitleService.UpdateAsync(request);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id) {
			var result = await _jobTitleService.DeleteAsync(id);
			return result ? Ok() : BadRequest();
		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<JobTitleResponseDto>> Enable(Guid id) {
			return await _jobTitleService.EnableAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<JobTitleResponseDto>> Disable(Guid id) {
			return await _jobTitleService.DisableAsync(id);
		}
	}
}
