﻿using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Contract;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Contract;

namespace QLHSNS.Services.IServices {
	public interface IContractService {
		Task<ApiResponse<ContractResponseDto>> GetContractByIdAsync(Guid id);
		Task<ApiResponse<PagedResult<ContractResponseDto>>> GetContractsAsync(PagingRequestBase request);
		Task<ApiResponse<ContractResponseDto>> CreateContractAsync(CreateContractRequestDto request);
		Task<ApiResponse<ContractResponseDto>> UpdateContractAsync(UpdateContractRequestDto request);
		Task<bool> DeleteAsync(Guid id);
		Task<ApiResponse<SoftDeleteContractResponseDto>> SoftDeleteAsync(Guid id);
		Task<ApiResponse<ContractResponseDto>> GetContractByEmployeeIdAsync(Guid id);
		Task<ApiResponse<List<string>>> UploadFilesAsync(Guid id, List<IFormFile> files);
		Task<ApiResponse<FileResponseDto>> DownloadFile(Guid id);
		Task<ApiResponse<PagedResult<ContractResponseDto>>> FilterAsync(FilterContractRequestDto request);
		Task<ApiResponse<List<AttachmentResponseDto>>> GetAllFileByContractIdAsync(Guid id);
		Task<ApiResponse<GetTotalEmployeeSalaryResponseDto>> GetTotalEmployeeSalary(GetTotalEmployeeSalaryRequestDto request);
	}
}
