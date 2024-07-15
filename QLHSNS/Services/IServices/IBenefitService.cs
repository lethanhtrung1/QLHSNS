using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Benefit;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Benefit;

namespace QLHSNS.Services.IServices {
	public interface IBenefitService {
		Task<ApiResponse<PagedResult<BenefitResponseDto>>> GetPagingAsync(PagingRequestBase request);
		Task<ApiResponse<BenefitResponseDto>> GetByIdAsync(Guid id);
		Task<ApiResponse<BenefitResponseDto>> CreateAsync(CreateBenefitRequestDto request);
		Task<ApiResponse<BenefitResponseDto>> DeleteAsync(Guid id);
		Task<ApiResponse<BenefitResponseDto>> UpdateAsync(UpdateBenefitRequestDto request);
		Task<ApiResponse<BenefitResponseDto>> EnableAsync(Guid id);
		Task<ApiResponse<BenefitResponseDto>> DisableAsync(Guid id);
		Task<ApiResponse<List<BenefitResponseDto>>> GetAllAsync();
	}
}
