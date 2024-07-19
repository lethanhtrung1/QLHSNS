using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Employee;
using QLHSNS.DTOs.Request.EmployeeRequestDto;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Employee;

namespace QLHSNS.Services.IServices {
	public interface IEmployeeService {
		Task<ApiResponse<PagedResult<EmployeeResponseDto>>> GetEmployeesAsync(PagingRequestBase request);
		Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(Guid id);
		Task<ApiResponse<EmployeeResponseDto>> CreateNewEmployeeAsync(CreateEmployeeRequestDto request);
		Task<ApiResponse<EmployeeAssetResponseDto>> CreateEmployeeAssetsAsync(EmployeeAssetRequestDto request);
		Task<ApiResponse<EmployeeResponseDto>> UpdateEmployeeAsyns(UpdateEmployeeRequestDto request);
		Task<bool> DeleteEmployeeAsync(Guid id);
		Task<ApiResponse<PagedResult<EmployeeResponseDto>>> SearchAllEmployeeAsync(EmployeePagingRequestDto request);
		Task<ApiResponse<EmployeeAssetResponseDto>> GetAssetByEmployeeId(Guid id);
	}
}
