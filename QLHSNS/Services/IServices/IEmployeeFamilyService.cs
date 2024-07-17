using QLHSNS.DTOs.Request.EmployeeFamily;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.EmployeeFamily;

namespace QLHSNS.Services.IServices {
	public interface IEmployeeFamilyService {
		Task<ApiResponse<List<EmployeeFamilyResponseDto>>> GetByEmployeeIdAsync(Guid id);
		Task<ApiResponse<EmployeeFamilyResponseDto>> DeleteAsync(Guid id);
		Task<ApiResponse<EmployeeFamilyResponseDto>> CreateAsync(CreateEmployeeFamilyRequestDto request);
		Task<ApiResponse<EmployeeFamilyResponseDto>> CreateRangeAsync(List<CreateEmployeeFamilyRequestDto> request);
		Task<ApiResponse<EmployeeFamilyResponseDto>> UpdateAsync(UpdateEmployeeFamilyRequestDto request);
		Task<ApiResponse<EmployeeFamilyResponseDto>> UpdateRangeAsync(List<UpdateEmployeeFamilyRequestDto> request);
	}
}
