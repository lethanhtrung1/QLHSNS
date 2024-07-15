using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.HealthCareRequestDto;
using QLHSNS.DTOs.Response;
using QLHSNS.Model;

namespace QLHSNS.Services.IServices {
	public interface IHealthCareService {
		Task<ApiResponse<HealthCare>> GetHealthCareByIdAsync(Guid id);
		Task<ApiResponse<PagedResult<HealthCare>>> GetHealthCaresAsync(PagingRequestBase request);
		Task<ApiResponse<HealthCare>> CreateHealthCareAsync(CreateHealthCareRequestDto request);
		Task<ApiResponse<HealthCare>> UpdateHealthCareAsync(UpdateHealthCareRequestDto request);
		Task<ApiResponse<HealthCare>> DeleteHealthCareAsync(Guid id);
		Task<ApiResponse<HealthCare>> EnableHealthCareAsync(Guid id);
		Task<ApiResponse<HealthCare>> DisableHealthCareAsync(Guid id);
	}
}
