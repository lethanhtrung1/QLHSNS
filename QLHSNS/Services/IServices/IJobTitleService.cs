using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.JobTitle;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.JobTitle;

namespace QLHSNS.Services.IServices {
	public interface IJobTitleService {
		Task<ApiResponse<JobTitleResponseDto>> GetByIdAsync(Guid id);
		Task<ApiResponse<PagedResult<JobTitleResponseDto>>> GetAllAsync(PagingRequestBase request);
		Task<ApiResponse<JobTitleResponseDto>> CreateAsync(CreateJobTitleRequestDto jobTitle);
		Task<ApiResponse<JobTitleResponseDto>> UpdateAsync(UpdateJobTitleRequestDto jobTitle);
		Task<bool> DeleteAsync(Guid id);
		Task<ApiResponse<JobTitleResponseDto>> EnableAsync(Guid id);
		Task<ApiResponse<JobTitleResponseDto>> DisableAsync(Guid id);
		Task<ApiResponse<List<JobTitleResponseDto>>> GetByDepartmentIdAsync(Guid departmentId);
	}
}
