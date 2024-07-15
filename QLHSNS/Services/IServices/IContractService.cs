using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Contract;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Contract;

namespace QLHSNS.Services.IServices {
	public interface IContractService {
		Task<ApiResponse<ContractResponseDto>> GetContractByIdAsync(Guid id);
		Task<ApiResponse<PagedResult<ContractResponseDto>>> GetContractsAsync(PagingRequestBase request);
		Task<ApiResponse<ContractResponseDto>> CreateContractAsync(CreateContractRequestDto request);
		Task<ApiResponse<ContractResponseDto>> UpdateContractAsync(UpdateContractRequestDto request);
		Task<ApiResponse<ContractResponseDto>> DeleteAsync(Guid id);
		Task<ApiResponse<SoftDeleteContractResponseDto>> SoftDeleteAsync(Guid id);
		Task<ApiResponse<ContractResponseDto>> GetContractByEmployeeIdAsync(Guid id);
		Task<IActionResult> UploadFile(Guid id, IFormFile file);
		Task<IActionResult> DownloadFile(Guid id);
		Task<ApiResponse<PagedResult<ContractResponseDto>>> Filter(FilterContractRequestDto request);
	}
}
