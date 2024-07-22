using QLHSNS.DTOs.Request.EmployeeFamily;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.EmployeeFamily;

namespace QLHSNS.Services.IServices {
	public interface IEmployeeFamilyService {
		Task<bool> DeleteAsync(Guid id);
		Task<ApiResponse<GetEmployeeFamilyWithDetailResponseDto>> GetByEmployeeIdAsync(Guid id);
		Task<ApiResponse<EmployeeFamilyResponseDto>> CreateAsync(CreateEmployeeFamilyRequestDto request);
		Task<ApiResponse<EmployeeFamilyResponseDto>> UpdateAsync(UpdateEmployeeFamilyRequestDto request);

		Task<ApiResponse<EmployeeFamilyDetailResponseDto>> AddEmployeeFamilyDetail(AddEmployeeFamilyDetailRequestDto request);
	}
}
