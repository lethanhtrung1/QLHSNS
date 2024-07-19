using QLHSNS.DTOs.Pagination;
using QLHSNS.DTOs.Request.Payroll;
using QLHSNS.DTOs.Response;
using QLHSNS.DTOs.Response.Payroll;

namespace QLHSNS.Services.IServices {
	public interface IPayrollService {
		Task<ApiResponse<PayrollResponseDto>> GetByIdAsync(Guid id);
		Task<ApiResponse<PagedResult<PayrollResponseDto>>> GetPayrollsAsync(PagingRequestBase request);
		Task<ApiResponse<PayrollResponseDto>> CreateAsync(CreatePayrollRequestDto request);
		Task<ApiResponse<PayrollResponseDto>> UpdateAsync(UpdatePayrollRequestDto request);
		Task<ApiResponse<PayrollResponseDto>> EnableAsync(Guid id);
		Task<ApiResponse<PayrollResponseDto>> DisableAsync(Guid id);
		Task<bool> DeleteAsync(Guid id);
		//Task<ApiResponse<PayrollResponseDto>> AddPayrollAllowanceAsync(AddPayrollAllowanceRequestDto request);
		//Task<ApiResponse<PayrollResponseDto>> AddPayrollBenefitAsync(AddPayrollBenefitRequestDto request);
	}
}
