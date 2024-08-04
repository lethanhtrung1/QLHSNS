using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Dashboard;
using QLHSNS.DTOs.Response;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class DashboardService : IDashboardService {
		private readonly AppDbContext _dbContext;

		public DashboardService(AppDbContext dbContext) {
			_dbContext = dbContext;
		}

		public async Task<ApiResponse<ReportApiResponseBase>> AllowanceChart() {
			var employees = _dbContext.Employees.Where(x => x.IsWorking == 1);

			var query = await (from e in employees
							   join c in _dbContext.Contracts.Where(x => x.IsDeleted == 0)
							   on e.Id equals c.EmployeeId
							   join p in _dbContext.Payrolls.Where(x => x.Status == 1)
							   on c.PayrollId equals p.Id
							   join pa in _dbContext.PayrollAllowances
							   on p.Id equals pa.PayrollId
							   join a in _dbContext.Allowances.Where(x => x.Status == 1)
							   on pa.AllowanceId equals a.Id
							   group a.Value by a.AllowanceName into temp
							   select new ReportData {
								   FieldName = temp.Key,
								   FieldValue = temp.Sum().ToString(),
							   }).ToListAsync();

			var result = new ReportApiResponseBase {
				NameChart = "Báo cáo phụ cấp theo tháng",
				Total = "",
				Data = query
			};

			return new ApiResponse<ReportApiResponseBase> {
				IsSuccess = true,
				Data = result
			};
		}

		public async Task<ApiResponse<ReportApiResponseBase>> BenefitChart() {
			var employees = _dbContext.Employees.Where(x => x.IsWorking == 1);

			var query = await (from e in employees
							   join c in _dbContext.Contracts.Where(x => x.IsDeleted == 0)
							   on e.Id equals c.EmployeeId
							   join p in _dbContext.Payrolls.Where(x => x.Status == 1)
							   on c.PayrollId equals p.Id
							   join pb in _dbContext.PayrollBenefits
							   on p.Id equals pb.PayrollId
							   join a in _dbContext.Benefits.Where(x => x.Status == 1)
							   on pb.BenefitId equals a.Id
							   group a.Amount by a.BenefitName into temp
							   select new ReportData {
								   FieldName = temp.Key,
								   FieldValue = temp.Sum().ToString(),
							   }).ToListAsync();

			var result = new ReportApiResponseBase {
				NameChart = "Báo cáo phúc lợi theo tháng",
				Total = "",
				Data = query
			};

			return new ApiResponse<ReportApiResponseBase> {
				IsSuccess = true,
				Data = result
			};
		}

		public async Task<ApiResponse<ReportApiResponseBase>> EmployeeChart(int year) {
			int newEmpployee = await _dbContext.Employees.Where(x => x.IsWorking == 1 && x.DateOfJoining.Year == year).CountAsync();
			int leaveEmployee = await _dbContext.Employees.Where(x => x.IsWorking == 0 && x.DateOfLeaving!.Value.Year == year).CountAsync();
			int totalEmployee = await _dbContext.Employees.CountAsync();

			var dataChart = new List<ReportData>();

			var newEmployeeReport = new ReportData {
				FieldName = "Nhân viên mới",
				FieldValue = newEmpployee.ToString(),
				Rate = (Math.Round((double)(newEmpployee / totalEmployee), 2)).ToString(),
			};

			var leaveEmployeeReport = new ReportData {
				FieldName = "Nhân viên đã nghỉ việc",
				FieldValue = leaveEmployee.ToString(),
				Rate = (Math.Round((double)(leaveEmployee / totalEmployee), 2)).ToString(),
			};

			dataChart.Add(newEmployeeReport);
			dataChart.Add(leaveEmployeeReport);

			var result = new ReportApiResponseBase {
				NameChart = "Báo cáo biến động nhân viên theo năm",
				Total = totalEmployee.ToString(),
				Data = dataChart
			};

			return new ApiResponse<ReportApiResponseBase> {
				Data = result,
				IsSuccess = true
			};
		}

		public async Task<ApiResponse<ReportApiResponseBase>> RewardChart(int month, int year) {
			var totalReward = await _dbContext.Rewards.Where(x => x.Month == month && x.Year == year).SumAsync(x => x.RewardAmount);

			var result = new ReportApiResponseBase {
				NameChart = "Báo cáo khen thưởng",
				Total = (Math.Round((double)(totalReward), 2)).ToString(),
			};

			return new ApiResponse<ReportApiResponseBase> {
				IsSuccess = true,
				Data = result,
			};
		}
	}
}
