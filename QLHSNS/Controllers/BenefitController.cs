using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Benefit;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Benefit;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BenefitController : ControllerBase {
		private readonly IBenefitService _service;

		public BenefitController(IBenefitService service) {
			_service = service;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<BenefitResponseDto>> GetById(Guid id) {
			return await _service.GetByIdAsync(id);
		}

		[HttpPost("GetList")]
		public async Task<ApiResponse<PagedResult<BenefitResponseDto>>> GetList(PagingRequestBase request) {
			return await _service.GetPagingAsync(request);
		}

		[HttpPost("Create")]
		public async Task<ApiResponse<BenefitResponseDto>> Create([FromBody] CreateBenefitRequestDto request) {
			return await _service.CreateAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<BenefitResponseDto>> Update([FromBody] UpdateBenefitRequestDto request) {
			return await _service.UpdateAsync(request);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<ApiResponse<BenefitResponseDto>> Delete(Guid id) {
			return await _service.DeleteAsync(id);
		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<BenefitResponseDto>> Enable(Guid id) {
			return await _service.EnableAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<BenefitResponseDto>> Disable(Guid id) {
			return await _service.DisableAsync(id);
		}

		[HttpGet("GetAll")]
		public async Task<ApiResponse<List<BenefitResponseDto>>> GetAll() {
			return await _service.GetAllAsync();
		}
	}
}
