using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.BankBranch;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BankBranchController : ControllerBase {
		private readonly IBankBranchService _service;

		public BankBranchController(IBankBranchService service) {
			_service = service;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<BankBranchResponseDto>> GetBankBranchById(Guid id) {
			return await _service.GetBankBranchByIdAsync(id);
		}

		[HttpGet("GetByBankId/{id:Guid}")]
		public async Task<ApiResponse<List<BankBranchDto>>> GetByBankId(Guid id) {
			return await _service.GetByBankIdAsync(id);
		}

		[HttpGet("GetAll")]
		public async Task<ApiResponse<List<BankBranchDto>>> GetAll() {
			return await _service.GetAllAsync();
		}

		[HttpPost("GetList")]
		public async Task<ApiResponse<PagedResult<BankBranchResponseDto>>> GetBankBranches(PagingRequestBase request) {
			return await _service.GetBankBranchesAsync(request);
		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<BankBranchResponseDto>> Enable(Guid id) {
			return await _service.EnableAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<BankBranchResponseDto>> Disable(Guid id) {
			return await _service.DisableAsync(id);
		}
	}
}
