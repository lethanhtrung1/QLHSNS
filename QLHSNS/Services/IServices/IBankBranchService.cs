﻿using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.BankBranch;

namespace QLHSNS.Services.IServices {
	public interface IBankBranchService {
		Task<ApiResponse<PagedResult<BankBranchResponseDto>>> GetBankBranchesAsync(PagingRequestBase request);
		Task<ApiResponse<BankBranchResponseDto>> GetBankBranchByIdAsync(Guid id);
		Task<ApiResponse<List<BankBranchDto>>> GetAllAsync(int status);
		Task<ApiResponse<List<BankBranchDto>>> GetByBankIdAsync(Guid BankId, int status);
		Task<ApiResponse<BankBranchResponseDto>> EnableAsync(Guid id);
		Task<ApiResponse<BankBranchResponseDto>> DisableAsync(Guid id);
	}
}
