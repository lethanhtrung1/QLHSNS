using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Contract;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Contract;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ContractController : ControllerBase {
		private readonly IContractService _service;

		public ContractController(IContractService service) {
			_service = service;
		}

		[HttpPost("CreateContract")]
		public async Task<ApiResponse<ContractResponseDto>> CreateContract(CreateContractRequestDto request) {
			return await _service.CreateContractAsync(request);
		}

		[HttpGet("GetContractById/{id:Guid}")]
		public async Task<ApiResponse<ContractResponseDto>> GetContractById(Guid id) {
			return await _service.GetContractByIdAsync(id);
		}

		[HttpGet("GetContractByEmployeeId/{id:Guid}")]
		public async Task<ApiResponse<ContractResponseDto>> GetContractByEmployeeId(Guid id) {
			return await _service.GetContractByEmployeeIdAsync(id);
		}

		[HttpDelete("Delete/{id:Guid}")]
		public async Task<IActionResult> DeleteContract(Guid id) {
			var result = await _service.DeleteAsync(id);
			return result ? Ok() : BadRequest();
		}

		[HttpPost("GetContractPaging")]
		public async Task<ApiResponse<PagedResult<ContractResponseDto>>> GetContractPaging(PagingRequestBase request) {
			return await _service.GetContractsAsync(request);
		}

		[HttpPut("SoftDelete/{id:Guid}")]
		public async Task<ApiResponse<SoftDeleteContractResponseDto>> SoftDelete(Guid id) {
			return await _service.SoftDeleteAsync(id);
		}

		[HttpPut("UpdateContract")]
		public async Task<ApiResponse<ContractResponseDto>> UpdateContract(UpdateContractRequestDto request) {
			return await _service.UpdateContractAsync(request);
		}

		[HttpPut("UploadFiles")]
		public async Task<ApiResponse<List<string>>> UploadFiles(Guid id, List<IFormFile> files) {
			return await _service.UploadFilesAsync(id, files);
		}

		[HttpGet("DownloadFile")]
		public async Task<IActionResult> DownloadFile(Guid id) {
			var data = await _service.DownloadFile(id);

			if (data.IsSuccess == true) {
				return File(data?.Data.ArchiveData, data?.Data.FileType, data?.Data.ArchiveName);
			}

			return NoContent();
		}

		[HttpGet("GetFilesByContractId/{id:Guid}")]
		public async Task<ApiResponse<List<AttachmentResponseDto>>> GetFilesByContractId(Guid id) {
			return await _service.GetAllFileByContractIdAsync(id);
		}

		[HttpPost("Filter")]
		public async Task<ApiResponse<PagedResult<ContractResponseDto>>> Filter(FilterContractRequestDto request) {
			return await _service.FilterAsync(request);
		}
	}
}
