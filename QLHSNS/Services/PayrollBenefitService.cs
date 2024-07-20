using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QLHSNS.Data;
using QLHSNS.DTOs.Request.Payroll;
using QLHSNS.Model;
using QLHSNS.Services.IServices;

namespace QLHSNS.Services {
	public class PayrollBenefitService : IPayrollBenefitService {
		private readonly AppDbContext _dbContext;
		private readonly IMapper _mapper;

		public PayrollBenefitService(AppDbContext dbContext, IMapper mapper) {
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<bool> AddPayrollBenefits(Guid payrollId, List<Guid> payrollBenefitIds) {
			var payrollFromDB = await _dbContext.Payrolls.Where(x => x.Id == payrollId && x.Status == 1).FirstOrDefaultAsync();

			if (payrollFromDB == null) {
				return false;
			}

			foreach (var item in payrollBenefitIds) {
				var payrollBenefit = new PayrollBenefitDto() {
					PayrollId = payrollId,
					BenefitId = item
				};
				var newPayrollBenefit = _mapper.Map<PayrollBenefit>(payrollBenefit);
				await _dbContext.PayrollBenefits.AddAsync(newPayrollBenefit);
			}
			await _dbContext.SaveChangesAsync();

			return true;
		}

		public async Task<bool> RemovePayrollBenefits(Guid payrollId, List<Guid> payrollBenefitIds) {
			var payrollBenefits = await _dbContext.PayrollBenefits.Where(x => x.PayrollId == payrollId).ToListAsync();

			if (payrollBenefits == null || payrollBenefits.Count == 0) {
				return false;
			}

			foreach (var item in payrollBenefits) {
				_dbContext.PayrollBenefits.Remove(item);
			}
			await _dbContext.SaveChangesAsync();

			return true;
		}
	}
}
