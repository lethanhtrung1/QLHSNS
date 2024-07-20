namespace QLHSNS.Services.IServices {
	public interface IPayrollBenefitService {
		Task<bool> AddPayrollBenefits(Guid payrollId, List<Guid> payrollBenefitIds);
		Task<bool> RemovePayrollBenefits(Guid payrollId, List<Guid> payrollBenefitIds);
	}
}
