using Microsoft.AspNetCore.Mvc;
using QLHSNS.DTOs.Dashboard;
using QLHSNS.DTOs.Response;
using QLHSNS.Services.IServices;

namespace QLHSNS.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class DashboardController : ControllerBase {
		private readonly IDashboardService _service;

		public DashboardController(IDashboardService service) {
			_service = service;
		}

		[HttpGet("Employee")]
		public async Task<ApiResponse<ReportApiResponseBase>> EmployeeReport(int year) {
			return await _service.EmployeeChart(year);
		}

		[HttpGet("Allowance")]
		public async Task<ApiResponse<ReportApiResponseBase>> AllowanceReport() {
			return await _service.AllowanceChart();
		}

		[HttpGet("Benefit")]
		public async Task<ApiResponse<ReportApiResponseBase>> BenefitReport() {
			return await _service.BenefitChart();
		}

		[HttpGet("Reward")]
		public async Task<ApiResponse<ReportApiResponseBase>> RewardReport(int month, int year) {
			return await _service.RewardChart(month, year);
		}
	}
}
