using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Payroll;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Payroll;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class PayrollController : ControllerBase {
		private readonly IPayrollService _service;

		public PayrollController(IPayrollService service) {
			_service = service;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<PayrollResponseDto>> GetById(Guid id) {
			return await _service.GetByIdAsync(id);
		}

		[HttpPost("GetPayrolls")]
		public async Task<ApiResponse<PagedResult<PayrollResponseDto>>> GetPayrolls(PagingRequestBase request) {
			return await _service.GetPayrollsAsync(request);
		}

		[HttpPost("Create")]
		public async Task<ApiResponse<PayrollResponseDto>> Create(CreatePayrollRequestDto request) {
			return await _service.CreateAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<PayrollResponseDto>> Update(UpdatePayrollRequestDto request) {
			return await _service.UpdateAsync(request);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<ApiResponse<PayrollResponseDto>> Delete(Guid id) {
			return await _service.DeleteAsync(id);
		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<PayrollResponseDto>> Enable(Guid id) {
			return await _service.EnableAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<PayrollResponseDto>> Disable(Guid id) {
			return await _service.DisableAsync(id);
		}
	}
}
