using QLHSNS.DTOs.Dashboard;
using QLHSNS.DTOs.Response;

namespace QLHSNS.Services.IServices {
	public interface IDashboardService {
		Task<ApiResponse<ReportApiResponseBase>> RewardChart(int month, int year);
		Task<ApiResponse<ReportApiResponseBase>> EmployeeChart(int year);
		Task<ApiResponse<ReportApiResponseBase>> AllowanceChart();
		Task<ApiResponse<ReportApiResponseBase>> BenefitChart();
	};
}
