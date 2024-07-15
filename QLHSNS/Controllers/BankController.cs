using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Bank;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BankController : ControllerBase {
		private readonly IBankService _service;

		public BankController(IBankService service) {
			_service = service;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<BankResponseDto>> GetBankById(Guid id) {
			return await _service.GetBankByIdAsync(id);
		}

		[HttpGet("GetAll")]
		public async Task<ApiResponse<List<BankResponseDto>>> GetAll() {
			return await _service.GetAllAsync();
		}

		[HttpPost("GetList")]
		public async Task<ApiResponse<PagedResult<BankResponseDto>>> GetBanks(PagingRequestBase request) {
			return await _service.GetBanksAsync(request);
		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<BankResponseDto>> Enable(Guid id) {
			return await _service.EnableAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<BankResponseDto>> Disable(Guid id) {
			return await _service.DisableAsync(id);
		}
	}
}
