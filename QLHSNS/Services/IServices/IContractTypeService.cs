﻿using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.ContractType;
using QLHSNS.DTOs.Response;
using QLHSNS.Model;

namespace QLHSNS.Services.IServices {
	public interface IContractTypeService {
		Task<ApiResponse<ContractType>> GetByIdAsync(Guid id);
		Task<ApiResponse<PagedResult<ContractType>>> GetPagingAsync(PagingRequestBase request);
		Task<ApiResponse<ContractType>> CreateAsync(CreateContractTypeRequestDto request);
		Task<ApiResponse<ContractType>> UpdateAsync(UpdateContractTypeRequestDto request);
		Task<bool> DeleteAsync(Guid id);
		Task<ApiResponse<ContractType>> EnableAsync(Guid id);
		Task<ApiResponse<ContractType>> DisableAsync(Guid id);
		Task<ApiResponse<List<ContractType>>> GetAllAsync(int status);
	}
}
