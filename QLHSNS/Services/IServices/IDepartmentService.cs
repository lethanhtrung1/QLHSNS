using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Department;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Department;

namespace QLHSNS.Services.IServices {
	public interface IDepartmentService {
		Task<ApiResponse<DepartmentResponseDto>> GetByIdAsync(Guid id);
		Task<ApiResponse<PagedResult<DepartmentResponseDto>>> GetDepartmentsAsync(PagingRequestBase request);
		Task<ApiResponse<DepartmentResponseDto>> CreateDepartmentAsync(CreateDepartmentRequestDto request);
		Task<ApiResponse<DepartmentResponseDto>> UpdateDepartmentAsync(UpdateDepartmentRequestDto request);
		Task<ApiResponse<DepartmentResponseDto>> DeleteDepartmentAsync(Guid id);
		Task<ApiResponse<DepartmentResponseDto>> EnableDepartmentAsync(Guid id);
		Task<ApiResponse<DepartmentResponseDto>> DisableDepartmentAsync(Guid id);
		Task<ApiResponse<List<DepartmentJobTitleResponseDto>>> AddDepartmentJobTitleAsync(DepartmentJobTitleRequestDto request);
		Task<ApiResponse<List<DepartmentJobTitleResponseDto>>> UpdateDepartmentJobTitleAsync(DepartmentJobTitleRequestDto request);
		Task<ApiResponse<List<DepartmentBaseResponseDto>>> GetAllAsync();
	}
}
