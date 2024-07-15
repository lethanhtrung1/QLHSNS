namespace QLHSNS.DTOs.Request.Payroll {
	public class AddPayrollBenefitRequestDto {
		public Guid PayrollId { get; set; }
		public List<Guid> BenefitIds { get; set; }
	}
}
