namespace QLHSNS.DTOs.Request.Payroll {
	public class AddPayrollAllowanceRequestDto {
		public Guid PayrollId { get; set; }
		public List<Guid> AllowanceIds { get; set; }
	}
}
