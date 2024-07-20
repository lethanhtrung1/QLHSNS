namespace QLHSNS.Services.IServices {
	public interface IPayrollAllowanceService {
		Task<bool> AddPayrollAllowances(Guid payrollId, List<Guid> payrollAllowanceIds);
		Task<bool> RemovePayrollAllowances(Guid payrollId, List<Guid> payrollAllowanceIds);
	}
}
