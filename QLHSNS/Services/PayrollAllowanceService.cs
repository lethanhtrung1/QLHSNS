using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Request.Payroll;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class PayrollAllowanceService : IPayrollAllowanceService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public PayrollAllowanceService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<bool> AddPayrollAllowances(Guid payrollId, List<Guid> payrollAllowanceIds) {
			var payrollFromDB = await _dbContext.Payrolls.Where(x => x.Id == payrollId && x.Status == 1).FirstOrDefaultAsync();

			if (payrollFromDB == null) {
				return false;
			}

			foreach (var item in payrollAllowanceIds) {
				var payrollAllowace = new PayrollAllowaceDto() {
					PayrollId = payrollId,
					AllowanceId = item
				};
				var newPayrollAllowace = _mapper.Map<PayrollAllowance>(payrollAllowace);
				await _dbContext.PayrollAllowances.AddAsync(newPayrollAllowace);
			}
			await _dbContext.SaveChangesAsync();

			return true;
		}

		public async Task<bool> RemovePayrollAllowances(Guid payrollId, List<Guid> payrollAllowanceIds) {
			var payrollAllowances = await _dbContext.PayrollAllowances.Where(x => x.PayrollId == payrollId).ToListAsync();

			if (payrollAllowances == null || payrollAllowances.Count == 0) {
				return false;
			}

			foreach (var item in payrollAllowances) {
				_dbContext.PayrollAllowances.Remove(item);
			}
			await _dbContext.SaveChangesAsync();

			return true;
		}
	}
}
