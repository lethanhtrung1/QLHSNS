using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.HealthCareRequestDto;
using QLHSNS.DTOs.Response;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class HealthCareController : ControllerBase {
		private readonly IHealthCareService _service;

		public HealthCareController(IHealthCareService service) {
			_service = service;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<HealthCare>> GetById(Guid id) {
			return await _service.GetHealthCareByIdAsync(id);
		}

		[HttpPost("GetList")]
		public async Task<ApiResponse<PagedResult<HealthCare>>> GetList(PagingRequestBase request) {
			return await _service.GetHealthCaresAsync(request);
		}

		[HttpPost("Create")]
		public async Task<ApiResponse<HealthCare>> Create(CreateHealthCareRequestDto request) {
			return await _service.CreateHealthCareAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<HealthCare>> Update(UpdateHealthCareRequestDto request) {
			return await _service.UpdateHealthCareAsync(request);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<ApiResponse<HealthCare>> Delete(Guid id) {
			return await _service.DeleteHealthCareAsync(id);
		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<HealthCare>> Enable(Guid id) {
			return await _service.EnableHealthCareAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<HealthCare>> Disable(Guid id) {
			return await _service.DisableHealthCareAsync(id);
		}
	}
}
