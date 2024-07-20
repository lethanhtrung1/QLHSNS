namespace QLHSNS.DTOs.Request.Payroll {
	public class AddPayrollAllowancesRequestDto {
		public Guid PayrollId { get; set; }
		public List<Guid> AllowanceIds { get; set; }
	}
}
