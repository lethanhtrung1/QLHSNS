using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Allowance;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Allowance;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AllowanceController : ControllerBase {
		private readonly IAllowanceService _allowanceService;

		public AllowanceController(IAllowanceService allowanceService) {
			_allowanceService = allowanceService;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<AllowanceResponseDto>> GetById(Guid id) {
			return await _allowanceService.GetByIdAsync(id);
		}

		[HttpPost("GetList")]
		public async Task<ApiResponse<PagedResult<AllowanceResponseDto>>> GetList(PagingRequestBase request) {
			return await _allowanceService.GetPagingAsync(request);
		}

		[HttpPost("Create")]
		public async Task<ApiResponse<AllowanceResponseDto>> Create(CreateAllowanceRequestDto request) {
			return await _allowanceService.CreateAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<AllowanceResponseDto>> Update(UpdateAllowanceRequestDto request) {
			return await _allowanceService.UpdateAsync(request);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id) {
			var result = await _allowanceService.DeleteAsync(id);
			return result ? Ok() : BadRequest();
		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<AllowanceResponseDto>> Enable(Guid id) {
			return await _allowanceService.EnableAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<AllowanceResponseDto>> Disable(Guid id) {
			return await _allowanceService.DisableAsync(id);
		}

		[HttpGet("GetAll")]
		public async Task<ApiResponse<List<AllowanceResponseDto>>> GetAll() {
			return await _allowanceService.GetAllAsync();
		}
	}
}
