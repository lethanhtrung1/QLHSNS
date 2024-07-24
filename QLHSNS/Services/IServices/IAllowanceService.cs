using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Allowance;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Allowance;
using QLHSNS.Model;

namespace QLHSNS.Services.IServices {
	public interface IAllowanceService {
		Task<ApiResponse<AllowanceResponseDto>> GetByIdAsync(Guid id);
		Task<ApiResponse<PagedResult<AllowanceResponseDto>>> GetPagingAsync(PagingRequestBase request);
		Task<ApiResponse<List<AllowanceResponseDto>>> GetAllAsync(int status);
		Task<ApiResponse<AllowanceResponseDto>> CreateAsync(CreateAllowanceRequestDto request);
		Task<ApiResponse<AllowanceResponseDto>> UpdateAsync(UpdateAllowanceRequestDto request);
		Task<bool> DeleteAsync(Guid id);
		Task<ApiResponse<AllowanceResponseDto>> EnableAsync(Guid id);
		Task<ApiResponse<AllowanceResponseDto>> DisableAsync(Guid id);
	}
}
