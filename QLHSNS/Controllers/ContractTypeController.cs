using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.ContractType;
using QLHSNS.DTOs.Response;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ContractTypeController : ControllerBase {
		private readonly IContractTypeService _constractTypeService;

		public ContractTypeController(IContractTypeService constractTypeService) {
			_constractTypeService = constractTypeService;
		}

		[HttpGet("GetById/{id:Guid}")]
		public async Task<ApiResponse<ContractType>> GetById(Guid id) {
			return await _constractTypeService.GetByIdAsync(id);
		}

		[HttpPost("GetAll")]
		public async Task<ApiResponse<PagedResult<ContractType>>> GetAll(PagingRequestBase request) {
			return await _constractTypeService.GetPagingAsync(request);
		}

		[HttpPost("Create")]
		public async Task<ApiResponse<ContractType>> Create(CreateContractTypeRequestDto request) {
			return await _constractTypeService.CreateAsync(request);
		}

		[HttpPut("Update")]
		public async Task<ApiResponse<ContractType>> Update(UpdateContractTypeRequestDto request) {
			return await _constractTypeService.UpdateAsync(request);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<IActionResult> Delete(Guid id) {
			var result = await _constractTypeService.DeleteAsync(id);
			return result ? Ok() : BadRequest();
 		}

		[HttpPut("Enable/{id:Guid}")]
		public async Task<ApiResponse<ContractType>> Enable(Guid id) {
			return await _constractTypeService.EnableAsync(id);
		}

		[HttpPut("Disable/{id:Guid}")]
		public async Task<ApiResponse<ContractType>> Disable(Guid id) {
			return await _constractTypeService.DisableAsync(id);
		}

		[HttpGet("GetAll/{status:int}")]
		public async Task<ApiResponse<List<ContractType>>> GetAll(int status) {
			return await _constractTypeService.GetAllAsync(status);
		}
	}
}
